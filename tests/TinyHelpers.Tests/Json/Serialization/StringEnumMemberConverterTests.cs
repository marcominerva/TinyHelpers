using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

namespace TinyHelpers.Tests.Json.Serialization;

public class StringEnumMemberConverterTests
{
#pragma warning disable CS0618
    [Fact]
    public void StringEnumMemberConverter_CanConvertEnumsAndNullableEnumsOnly()
    {
        var sut = new StringEnumMemberConverter();

        Assert.True(sut.CanConvert(typeof(Sample)));
        Assert.True(sut.CanConvert(typeof(Sample?)));
        Assert.False(sut.CanConvert(typeof(string)));
    }

    [Fact]
    public void StringEnumMemberConverter_UsesEnumMemberAndNamingPolicyAndReadsCaseInsensitively()
    {
        var options = Options(new StringEnumMemberConverter(JsonNamingPolicy.CamelCase, true));

        Assert.Equal("\"custom-value\"", JsonSerializer.Serialize(Sample.Custom, options));
        Assert.Equal("\"otherValue\"", JsonSerializer.Serialize(Sample.OtherValue, options));
        Assert.Equal(Sample.Custom, JsonSerializer.Deserialize<Sample>("\"CUSTOM-VALUE\"", options));
        Assert.Equal(Sample.OtherValue, JsonSerializer.Deserialize<Sample>("\"OTHERVALUE\"", options));
    }

    [Fact]
    public void StringEnumMemberConverter_Flags_RoundTripsCompositeAndRejectsUnknownFlag()
    {
        var options = Options(new StringEnumMemberConverter());
        var value = SampleFlags.Read | SampleFlags.Write;

        var json = JsonSerializer.Serialize(value, options);

        Assert.Equal("\"read, Write\"", json);
        Assert.Equal(value, JsonSerializer.Deserialize<SampleFlags>("\"READ, write\"", options));
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<SampleFlags>("\"read, Missing\"", options));
    }

    [Fact]
    public void StringEnumMemberConverter_IntegerBehavior_HonorsSetting()
    {
        var allowed = Options(new StringEnumMemberConverter(null, true));
        var denied = Options(new StringEnumMemberConverter(null, false));

        Assert.Equal("99", JsonSerializer.Serialize((Sample)99, allowed));
        Assert.Equal((Sample)99, JsonSerializer.Deserialize<Sample>("99", allowed));
        Assert.Throws<JsonException>(() => JsonSerializer.Serialize((Sample)99, denied));
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Sample>("99", denied));
    }

    [Fact]
    public void StringEnumMemberConverter_NullableEnum_RoundTripsValueAndNull()
    {
        var options = Options(new StringEnumMemberConverter());

        Assert.Equal(Sample.Custom, JsonSerializer.Deserialize<Sample?>("\"custom-value\"", options));
        Assert.Null(JsonSerializer.Deserialize<Sample?>("null", options));
    }
#pragma warning restore CS0618

    private static JsonSerializerOptions Options(JsonConverter converter)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(converter);
        return options;
    }

    private enum Sample
    {
        [EnumMember(Value = "custom-value")]
        Custom,
        OtherValue
    }

    [Flags]
    private enum SampleFlags
    {
        None = 0,
        [EnumMember(Value = "read")]
        Read = 1,
        Write = 2
    }
}
