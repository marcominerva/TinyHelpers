using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization;

/// <summary>
/// Converts <see cref="TimeSpan" /> values to JSON ticks so durations can be round-tripped without format ambiguity.
/// </summary>
/// <seealso cref="TimeSpan"/>
public class TimeSpanTicksConverter : JsonConverter<TimeSpan>
{
    /// <inheritdoc/>
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => TimeSpan.FromTicks(reader.GetInt64());

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.Ticks);
}
