using System.Text.Json;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

namespace TinyHelpers.Tests.Json.Serialization;

public class ShortDateConverterTests
{
    [Fact]
    public void ShortDateConverter_DefaultFormat_RoundTripsDate()
    {
        var options = Options(new ShortDateConverter());

        var json = JsonSerializer.Serialize(new DateTime(2026, 9, 9, 18, 30, 0), options);
        var result = JsonSerializer.Deserialize<DateTime>(json, options);

        Assert.Equal("\"2026-09-09\"", json);
        Assert.Equal(new DateTime(2026, 9, 9), result);
    }

    [Fact]
    public void ShortDateConverter_CustomFormat_WritesRequestedFormat()
    {
        Assert.Equal("\"09/09/2026\"", JsonSerializer.Serialize(new DateTime(2026, 9, 9), Options(new ShortDateConverter("dd/MM/yyyy"))));
    }

    private static JsonSerializerOptions Options(JsonConverter converter)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(converter);
        return options;
    }
}
