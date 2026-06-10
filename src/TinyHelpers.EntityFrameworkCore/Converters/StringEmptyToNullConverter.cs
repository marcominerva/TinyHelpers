using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

/// <summary>
/// Normalizes blank strings to <see langword="null" /> before persistence.
/// </summary>
/// <remarks>
/// This is useful when the database should treat empty input as an absence of value, which simplifies filtering
/// and avoids storing meaningless whitespace-only data.
/// </remarks>
public class StringEmptyToNullConverter() : ValueConverter<string?, string?>(
            value => string.IsNullOrWhiteSpace(value) ? null : value,
            value => value);
