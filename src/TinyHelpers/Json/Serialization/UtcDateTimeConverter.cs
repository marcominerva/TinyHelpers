using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization;

/// <summary>
/// A converter for serializing and deserializing <see cref="DateTime"/> values converting them to UTC, if needed.
/// </summary>
/// <seealso cref="DateTime"/>
/// <remarks>
/// Initializes a new instance of the <see cref="UtcDateTimeConverter"/> class with a specified serialization format.
/// </remarks>
/// <param name="serializationFormat">The serialization format to use. The default is yyyy-MM-ddTHH:mm:ss.fffffffZ".</param>
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
