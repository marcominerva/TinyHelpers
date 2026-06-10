using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TinyHelpers.EntityFrameworkCore.Converters;

namespace TinyHelpers.EntityFrameworkCore.Comparers;

/// <summary>
/// Compares values by their JSON representation so Entity Framework Core change tracking uses serialized equivalence instead of
/// reference equality.
/// </summary>
/// <typeparam name="T">The CLR type to compare.</typeparam>
/// <param name="jsonSerializerOptions">
/// The serializer options to use when producing the comparison payload. If <see langword="null" />, the
/// library defaults are used.
/// </param>
/// <remarks>
/// This comparer is intended for properties persisted through <see cref="JsonStringConverter{T}" /> so the
/// change tracker sees two objects as equal when their serialized payloads are the same, which prevents redundant
/// updates for value objects and complex graphs.
/// </remarks>
public class JsonStringComparer<T>(JsonSerializerOptions? jsonSerializerOptions = null) : ValueComparer<T?>(
        (first, second) => Serialize(first, jsonSerializerOptions) == Serialize(second, jsonSerializerOptions),
        value => GetHashCode(value, jsonSerializerOptions))
{
    private static string Serialize(T? value, JsonSerializerOptions? jsonSerializerOptions)
    {
        return JsonSerializer.Serialize<object?>(value, jsonSerializerOptions ?? JsonOptions.Default);
    }

    private static int GetHashCode(T? value, JsonSerializerOptions? jsonSerializerOptions)
    {
        if (value is null)
        {
            return 0;
        }

        return StringComparer.Ordinal.GetHashCode(Serialize(value, jsonSerializerOptions));
    }
}
