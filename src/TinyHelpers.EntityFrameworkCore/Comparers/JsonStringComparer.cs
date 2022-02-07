using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TinyHelpers.EntityFrameworkCore.Comparers;

public class JsonStringComparer<T> : ValueComparer<T?>
{
    public JsonStringComparer(JsonSerializerOptions? jsonSerializerOptions = null) : base(
        (first, second) => JsonSerializer.Serialize<object?>(first, jsonSerializerOptions ?? JsonOptions.Default) == JsonSerializer.Serialize<object?>(second, jsonSerializerOptions ?? JsonOptions.Default),
        value => value == null ? 0 : value.GetHashCode())
    {
    }
}
