using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

/// <summary>
/// Normalizes blank strings to <see langword="null" /> and trims meaningful values before persistence.
/// </summary>
/// <remarks>
/// Use this converter when user input should not keep incidental leading or trailing whitespace, but the model
/// still needs to preserve the difference between no value and a real string.
/// </remarks>
public class StringEmptyToNullTrimConverter() : ValueConverter<string?, string?>(
            value => string.IsNullOrWhiteSpace(value) ? null : value.Trim(),
            value => value);
