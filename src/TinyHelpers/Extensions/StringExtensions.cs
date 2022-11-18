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

    public static string ReplaceIgnoreCase(this string input, string pattern, string replacement)
        => Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);

    public static string? GetValueOrDefault(this string? input)
        => input.GetValueOrDefault(defaultValue: default, whitespaceAsEmpty: true);

    public static string? GetValueOrDefault(this string? input, string? defaultValue)
        => input.GetValueOrDefault(defaultValue, whitespaceAsEmpty: true);

    public static string? GetValueOrDefault(this string? input, string? defaultValue, bool whitespaceAsEmpty)
        => whitespaceAsEmpty ? (string.IsNullOrWhiteSpace(input) ? defaultValue : input) : (string.IsNullOrEmpty(input) ? defaultValue : input);

    public static bool HasValue(this string? input)
        => input.HasValue(allowEmptyString: false);

    public static bool HasValue(this string? input, bool allowEmptyString)
        => !(allowEmptyString ? input is null : string.IsNullOrWhiteSpace(input));
}
