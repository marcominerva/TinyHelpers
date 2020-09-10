using System;

namespace TinyNetHelpers.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string? a, string? b)
            => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

        public static string? GetValueOrDefault(this string? input, string? defaultValue = default)
            => string.IsNullOrWhiteSpace(input) ? defaultValue : input;
    }
}
