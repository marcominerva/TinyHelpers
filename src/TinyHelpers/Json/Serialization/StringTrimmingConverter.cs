using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization;

/// <summary>
/// Trims leading and trailing whitespace from JSON string values during serialization and deserialization.
/// </summary>
/// <remarks>
/// Use this converter when the JSON boundary should normalize user-entered text before it reaches the domain model or
/// before values are written back to clients.
/// </remarks>
public class StringTrimmingConverter : JsonConverter<string>
{
    /// <inheritdoc/>
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()?.Trim();

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        => writer.WriteStringValue(value?.Trim());
}
