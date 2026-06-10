using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

/// <summary>
/// Converts a CLR object graph to and from a JSON string for storage in a text column.
/// </summary>
/// <typeparam name="T">The CLR type to serialize.</typeparam>
/// <param name="jsonSerializerOptions">
/// The serializer options to use for both serialization and deserialization. If <see langword="null" />, the
/// library defaults are used.
/// </param>
/// <remarks>
/// The converter centralizes JSON persistence so entity properties can stay strongly typed while the database
/// only stores text. It is typically used together with <see cref="JsonStringComparer{T}" /> to keep Entity Framework Core
/// change tracking aligned with the serialized payload.
/// </remarks>
public class JsonStringConverter<T>(JsonSerializerOptions? jsonSerializerOptions = null) : ValueConverter<T?, string>(
        value => JsonSerializer.Serialize<object?>(value, jsonSerializerOptions ?? JsonOptions.Default),
        json => JsonSerializer.Deserialize<T>(json, jsonSerializerOptions ?? JsonOptions.Default));
