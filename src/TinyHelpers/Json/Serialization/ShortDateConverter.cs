using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization;

/// <summary>
/// Converts a <see cref="DateTime"/> value to or from JSON, keeping only the date part.
/// </summary>
/// <seealso cref="DateTime"/>
public class ShortDateConverter : JsonConverter<DateTime>
{
    private readonly string serializationFormat;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShortDateConverter"/> class.
    /// </summary>
    public ShortDateConverter() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShortDateConverter"/> class with a specified serialization format.
    /// </summary>
    /// <param name="serializationFormat">The serialization format to use. The default is yyyy-MM-dd.</param>
    public ShortDateConverter(string? serializationFormat)
    {
        this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
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
