using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TinyHelpers.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string? a, string? b)
        => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

    public static bool StartsWithIgnoreCase(this string? a, string b)
        => a?.StartsWith(b, StringComparison.OrdinalIgnoreCase) ?? false;

    public static bool EndsWithIgnoreCase(this string? a, string b)
        => a?.EndsWith(b, StringComparison.OrdinalIgnoreCase) ?? false;

    public static bool ContainsIgnoreCase(this string? a, string b)
        => a?.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0;

    /// <summary>
    /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string, ignoring the letter casing.
    /// </summary>
    /// <param name="input">The original string</param>
    /// <param name="pattern">The string to be replaced, case insentive.</param>
    /// <param name="replacement">The string to replace all occurrences of <paramref name="pattern"/> </param>
    /// <returns>A string that is equivalent to the <paramref name="input"/> string except that all instances of <paramref name="pattern"/> are replaced with <paramref name="replacement"/>.</returns>
    public static string ReplaceIgnoreCase(this string input, string pattern, string replacement)
        => Regex.Replace(input, Regex.Escape(pattern), replacement, RegexOptions.IgnoreCase);

    [return: NotNullIfNotNull(nameof(input))]
    public static string? GetValueOrDefault(this string? input)
        => input.GetValueOrDefault(defaultValue: default, whiteSpaceAsEmpty: true);

    [return: NotNullIfNotNull(nameof(input))]
    public static string? GetValueOrDefault([NotNullIfNotNull(nameof(input))] this string? input, string? defaultValue)
        => input.GetValueOrDefault(defaultValue, whiteSpaceAsEmpty: true);

    [return: NotNullIfNotNull(nameof(input))]
    public static string? GetValueOrDefault([NotNullIfNotNull(nameof(input))] this string? input, string? defaultValue, bool whiteSpaceAsEmpty)
        => whiteSpaceAsEmpty ? (string.IsNullOrWhiteSpace(input) ? defaultValue : input) : (string.IsNullOrEmpty(input) ? defaultValue : input);

    /// <summary>
    /// Checks whether the given string contains an actual value, not allowing empty or whitespace values.
    /// </summary>
    /// <param name="input">The string to be validated</param>
    /// <returns><see langword="true"/> if the string has a value, <see langword="false"/> otherwise</returns>
    public static bool HasValue([NotNullWhen(true)] this string? input)
        => input.HasValue(allowEmptyString: false, whiteSpaceAsEmpty: true);

    /// <summary>
    /// Checks whether the given string contains an actual value, allowing to specify if permitting empty strings, and treating whitespace strings as empty.
    /// </summary>
    /// <param name="input">The string to be validated</param>
    /// <param name="allowEmptyString"><see langword="true"/> to allow empty string, <see langword="false"/> otherwise</param>
    /// <returns><see langword="true"/> if the string has a value, <see langword="false"/> otherwise</returns>
    public static bool HasValue([NotNullWhen(true)] this string? input, bool allowEmptyString)
        => input.HasValue(allowEmptyString, whiteSpaceAsEmpty: true);

    /// <summary>
    /// Checks whether the given string contains an actual value, allowing to specify if permitting empty strings and if treating whitespace strings as empty.
    /// </summary>
    /// <param name="input">The string to be validated</param>
    /// <param name="allowEmptyString"><see langword="true"/> to allow empty string, <see langword="false"/> otherwise</param>
    /// <param name="whiteSpaceAsEmpty"><see langword="true"/> if whitespace should be considered as empty string, <see langword="false"/> otherwise</param>
    /// <returns><see langword="true"/> if the string has a value, <see langword="false"/> otherwise</returns>
    public static bool HasValue([NotNullWhen(true)] this string? input, bool allowEmptyString, bool whiteSpaceAsEmpty)
        => allowEmptyString ? input is not null : whiteSpaceAsEmpty ? !string.IsNullOrWhiteSpace(input) : !string.IsNullOrEmpty(input);
}