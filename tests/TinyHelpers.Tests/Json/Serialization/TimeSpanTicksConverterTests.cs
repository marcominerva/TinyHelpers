using System.Text.Json;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

namespace TinyHelpers.Tests.Json.Serialization;

public class TimeSpanTicksConverterTests
{
    [Fact]
    public void TimeSpanTicksConverter_RoundTripsPositiveAndNegativeTicks()
    {
        var options = Options(new TimeSpanTicksConverter());
        var value = TimeSpan.FromTicks(-123456789);

        var json = JsonSerializer.Serialize(value, options);

        Assert.Equal("-123456789", json);
        Assert.Equal(value, JsonSerializer.Deserialize<TimeSpan>(json, options));
    }

    private static JsonSerializerOptions Options(JsonConverter converter)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(converter);
        return options;
    }
}
