using System.Text.Json;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

namespace TinyHelpers.Tests.Json.Serialization;

public class StringTrimmingConverterTests
{
    [Theory]
    [InlineData("  value  ", "value")]
    [InlineData("   ", "")]
    public void StringTrimmingConverter_RoundTripsTrimmedValue(string value, string expected)
    {
        var options = Options(new StringTrimmingConverter());

        var json = JsonSerializer.Serialize(value, options);

        Assert.Equal(expected, JsonSerializer.Deserialize<string>(json, options));
        Assert.Equal(JsonSerializer.Serialize(expected), json);
    }

    private static JsonSerializerOptions Options(JsonConverter converter)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(converter);
        return options;
    }
}
