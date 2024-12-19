using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

public class JsonStringConverter<T>(JsonSerializerOptions? jsonSerializerOptions = null) : ValueConverter<T?, string>(
        value => JsonSerializer.Serialize<object?>(value, jsonSerializerOptions ?? JsonOptions.Default),
        json => JsonSerializer.Deserialize<T>(json, jsonSerializerOptions ?? JsonOptions.Default))
{
}