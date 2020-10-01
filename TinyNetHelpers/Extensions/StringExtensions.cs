using System;

namespace TinyNetHelpers.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string? a, string? b)
            => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

        public static bool StartsWithIgnoreCase(this string? a, string? b)
            => a?.StartsWith(b, StringComparison.OrdinalIgnoreCase) ?? false;

        public static bool EndsWithIgnoreCase(this string? a, string? b)
            => a?.EndsWith(b, StringComparison.OrdinalIgnoreCase) ?? false;

        public static bool ContainsIgnoreCase(this string? a, string? b)
            => a?.Contains(b, StringComparison.OrdinalIgnoreCase) ?? false;

        public static string? GetValueOrDefault(this string? input, string? defaultValue = default, bool whitespaceAsEmpty = true)
            => whitespaceAsEmpty ? (string.IsNullOrWhiteSpace(input) ? defaultValue : input) : (string.IsNullOrEmpty(input) ? defaultValue : input);
    }
}
