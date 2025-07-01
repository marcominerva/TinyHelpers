using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TinyHelpers.Extensions;

/// <summary>
/// Contains extensions methods for the <see cref="string"/> type.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Determines whether two specified <see cref="string"/> objects have the same value, performing a case-insensitive comparison.
    /// </summary>
    /// <param name="a">The first string to compare.</param>
    /// <param name="b">The second string to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="a"/> is the same as the value of <paramref name="b"/>, regardless the casing; otherwise, <see langword="false"/>. If both <paramref name="a"/> and <paramref name="b"/> are <see langword="null"/>, the method returns <see langword="true"/>.</returns>
    public static bool EqualsIgnoreCase(this string? a, string? b)
        => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Determines whether the beginning of this string instance matches the specified string, performing a case-insensitive comparison.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <param name="value">The string to compare.</param>
    /// <returns><see langword="true"/> if this instance begins with <paramref name="value"/>, regardless the casing; otherwise, <see langword="false"/>. If <paramref name="input"/> is <see langword="null"/>, the method returns <see langword="false"/>.</returns>
    public static bool StartsWithIgnoreCase(this string? input, string value)
        => input?.StartsWith(value, StringComparison.OrdinalIgnoreCase) ?? false;

    /// <summary>
    /// Determines whether the end of this string instance matches the specified string, performing a case-insensitive comparison.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <param name="value">The string to compare.</param>
    /// <returns><see langword="true"/> if this instance ends with <paramref name="value"/>, regardless the casing; otherwise, <see langword="false"/>. If <paramref name="input"/> is <see langword="null"/>, the method returns <see langword="false"/>.</returns>
    public static bool EndsWithIgnoreCase(this string? input, string value)
        => input?.EndsWith(value, StringComparison.OrdinalIgnoreCase) ?? false;

    /// <summary>
    /// Determines whether this string instance contains the specified string, performing a case-insensitive comparison.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <param name="value">The string to compare.</param>
    /// <returns><see langword="true"/> if this instance contains <paramref name="value"/>, regardless the casing; otherwise, <see langword="false"/>. If <paramref name="input"/> is <see langword="null"/>, the method returns <see langword="false"/>.</returns>
    public static bool ContainsIgnoreCase(this string? input, string value)
        => input?.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

    /// <summary>
    /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string, ignoring the letter casing.
    /// </summary>
    /// <param name="input">The original string.</param>
    /// <param name="pattern">The string to be replaced, case insensitive.</param>
    /// <param name="replacement">The string to be used to replace all occurrences of <paramref name="pattern"/>.</param>
    /// <returns>A string that is equivalent to the <paramref name="input"/> string except that all instances of <paramref name="pattern"/> are replaced with <paramref name="replacement"/>.</returns>
    public static string ReplaceIgnoreCase(this string input, string pattern, string replacement)
        => Regex.Replace(input, Regex.Escape(pattern), replacement, RegexOptions.IgnoreCase);

    /// <summary>
    /// Gets the actual value of this string instance, or <see langword="null"/> if the string is <see langword="null"/> or contains only whitespaces.
    /// </summary>
    /// <param name="input">The string to test.</param>
    /// <returns>The actual value of this string instance, or <see langword="null"/> if the string is <see langword="null"/> or contains only whitespaces.</returns>
    [return: NotNullIfNotNull(nameof(input))]
    public static string? GetValueOrDefault(this string? input)
        => input.GetValueOrDefault(defaultValue: default, whiteSpaceAsEmpty: true);

    /// <summary>
    /// Gets the actual value of this string instance, or the specified <paramref name="defaultValue"/> if the string is <see langword="null"/> or contains only whitespaces.
    /// </summary>
    /// <param name="input">The string to test.</param>
    /// <param name="defaultValue">The value to return if the string is <see langword="null"/> or contains only whitespaces.</param>
    /// <returns>The actual value of this string instance, or <paramref name="defaultValue"/> if the string is <see langword="null"/> or contains only whitespaces.</returns>
    [return: NotNullIfNotNull(nameof(input))]
    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static string? GetValueOrDefault(this string? input, string? defaultValue)
        => input.GetValueOrDefault(defaultValue, whiteSpaceAsEmpty: true);

    /// <summary>
    /// Gets the actual value of this string instance, or the specified <paramref name="defaultValue"/> if the string is <see langword="null"/>, allowing to specify whether whitespaces must be considered as empty characters.
    /// </summary>
    /// <param name="input">The string to test.</param>
    /// <param name="defaultValue">The value to return if the string is <see langword="null"/> or empty according to the value of <paramref name="whiteSpaceAsEmpty"/> parameter.</param>
    /// <param name="whiteSpaceAsEmpty"><see langword="true"/> if whitespaces must be considered as empty characters; otherwise, <see langword="false"/>.</param>
    /// <returns>The actual value of this string instance, or <paramref name="defaultValue"/> if the string is <see langword="null"/> or contains only whitespaces.</returns>
    [return: NotNullIfNotNull(nameof(input))]
    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static string? GetValueOrDefault(this string? input, string? defaultValue, bool whiteSpaceAsEmpty)
        => whiteSpaceAsEmpty ? (string.IsNullOrWhiteSpace(input) ? defaultValue : input) : (string.IsNullOrEmpty(input) ? defaultValue : input);

    /// <summary>
    /// Checks whether the given string contains an actual value, not allowing empty or whitespace values.
    /// </summary>
    /// <param name="input">The string to test.</param>
    /// <returns><see langword="true"/> if the string has a value; otherwise, <see langword="false"/>.</returns>
    public static bool HasValue([NotNullWhen(true)] this string? input)
        => input.HasValue(allowEmptyString: false, whiteSpaceAsEmpty: true);

    /// <summary>
    /// Checks whether the given string contains an actual value, allowing to specify if permitting empty strings, and treating whitespace strings as empty.
    /// </summary>
    /// <param name="input">The string to test.</param>
    /// <param name="allowEmptyString"><see langword="true"/> to allow empty string, <see langword="false"/> otherwise.</param>
    /// <returns><see langword="true"/> if the string has a value; otherwise, <see langword="false"/>.</returns>
    public static bool HasValue([NotNullWhen(true)] this string? input, bool allowEmptyString)
        => input.HasValue(allowEmptyString, whiteSpaceAsEmpty: true);

    /// <summary>
    /// Checks whether the given string contains an actual value, allowing to specify if permitting empty strings and if treating whitespace strings as empty.
    /// </summary>
    /// <param name="input">The string to test.</param>
    /// <param name="allowEmptyString"><see langword="true"/> to allow empty string, <see langword="false"/> otherwise.</param>
    /// <param name="whiteSpaceAsEmpty"><see langword="true"/> if whitespace should be considered as empty string, <see langword="false"/> otherwise.</param>
    /// <returns><see langword="true"/> if the string has a value; otherwise, <see langword="false"/>.</returns>
    public static bool HasValue([NotNullWhen(true)] this string? input, bool allowEmptyString, bool whiteSpaceAsEmpty)
        => allowEmptyString ? input is not null : whiteSpaceAsEmpty ? !string.IsNullOrWhiteSpace(input) : !string.IsNullOrEmpty(input);

    /// <summary>
    /// Creates a new string in which the first character is uppercase.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The string with the first character uppercase and all the other characters as in the original input.</returns>
    public static string FirstCharToUpper(this string input)
    {
#if NETSTANDARD2_0
        var result = input switch
        {
            null or "" => string.Empty,
            _ => $"{input[0].ToString().ToUpper()}{input.Substring(1)}"
        };
#else
        var result = input switch
        {
            null or "" => string.Empty,
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
#endif

        return result;
    }
}