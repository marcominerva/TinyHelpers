using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization;

/// <summary>
/// Converts <see cref="DateTime" /> values to UTC during JSON serialization and deserialization.
/// </summary>
/// <seealso cref="DateTime"/>
/// <remarks>
/// Use this converter when a JSON contract should normalize date-time values to a UTC wire format instead of preserving
/// local offsets or unspecified kinds.
/// </remarks>
/// <param name="serializationFormat">The serialization format to use. The default is <c>yyyy-MM-ddTHH:mm:ss.fffffffZ</c>.</param>
/// <seealso cref="UtcDateTimeConverter"/>
public class UtcDateTimeConverter(string? serializationFormat) : JsonConverter<DateTime>
{
    private readonly string serializationFormat = serializationFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'";

    /// <summary>
    /// Initializes a new instance of the <see cref="UtcDateTimeConverter"/> class.
    /// </summary>
    /// <seealso cref="UtcDateTimeConverter"/>
    public UtcDateTimeConverter() : this(null)
    {
    }

    /// <inheritdoc/>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDateTime().ToUniversalTime();

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue((value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : value)
            .ToString(serializationFormat));
}
