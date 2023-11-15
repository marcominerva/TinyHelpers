using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization;

/// <summary>
/// Converts a <see cref="DateTime"/> value to or from JSON, keeping only the date part.
/// </summary>
/// <seealso cref="DateTime"/>
/// <remarks>
/// Initializes a new instance of the <see cref="ShortDateConverter"/> class with a specified serialization format.
/// </remarks>
/// <param name="serializationFormat">The serialization format to use. The default is yyyy-MM-dd.</param>
public class ShortDateConverter(string? serializationFormat) : JsonConverter<DateTime>
{
    private readonly string serializationFormat = serializationFormat ?? "yyyy-MM-dd";

    /// <summary>
    /// Initializes a new instance of the <see cref="ShortDateConverter"/> class.
    /// </summary>
    public ShortDateConverter() : this(null)
    {
    }

    /// <inheritdoc/>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return DateTimeOffset.Parse(value!, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal).Date;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(serializationFormat));
}
