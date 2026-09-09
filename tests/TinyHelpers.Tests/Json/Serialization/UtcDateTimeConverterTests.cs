using System.Text.Json;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

namespace TinyHelpers.Tests.Json.Serialization;

public class UtcDateTimeConverterTests
{
    [Fact]
    public void UtcDateTimeConverter_DefaultFormat_WritesUtcAndReadsUtc()
    {
        var options = Options(new UtcDateTimeConverter());
        var value = new DateTime(2026, 9, 9, 10, 23, 45, DateTimeKind.Utc).AddTicks(1234567);

        var json = JsonSerializer.Serialize(value, options);
        var result = JsonSerializer.Deserialize<DateTime>(json, options);

        Assert.Equal("\"2026-09-09T10:23:45.1234567Z\"", json);
        Assert.Equal(value, result);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void UtcDateTimeConverter_CustomFormat_WritesRequestedFormat()
    {
        var options = Options(new UtcDateTimeConverter("yyyyMMdd"));

        Assert.Equal("\"20260909\"", JsonSerializer.Serialize(new DateTime(2026, 9, 9, 0, 0, 0, DateTimeKind.Utc), options));
    }

    private static JsonSerializerOptions Options(JsonConverter converter)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(converter);
        return options;
    }
}
