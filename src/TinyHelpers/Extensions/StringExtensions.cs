using System.Text;
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

    public static string? GetValueOrDefault(this string? input, string? defaultValue = default, bool whitespaceAsEmpty = true)
        => whitespaceAsEmpty ? (string.IsNullOrWhiteSpace(input) ? defaultValue : input) : (string.IsNullOrEmpty(input) ? defaultValue : input);

    public static string Repeat(this string str, int times)
    {
        var s = new StringBuilder();
        for (int i = 0; i < times; i++)
        {
            s.Append(str);
        }
        return s.ToString();
    }
}
