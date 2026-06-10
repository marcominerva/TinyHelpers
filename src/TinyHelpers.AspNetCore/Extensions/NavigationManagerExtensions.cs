using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace TinyHelpers.AspNetCore.Extensions;

/// <summary>
/// Adds query-string parsing helpers that keep component code focused on behavior instead of URI manipulation.
/// </summary>
public static class NavigationManagerExtensions
{
    /// <summary>
    /// Tries to read a typed value from the current URI query string without requiring callers to parse the raw string themselves.
    /// </summary>
    /// <typeparam name="T">The expected query-string value type.</typeparam>
    /// <param name="navManager">The navigation manager that exposes the current URI.</param>
    /// <param name="key">The query-string key to match.</param>
    /// <param name="value">When this method returns, contains the converted value if the key exists and can be parsed.</param>
    /// <returns><see langword="true" /> when the query string contains a parsable value for <paramref name="key" />; otherwise, <see langword="false" />.</returns>
    public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, [NotNullWhen(true)] out T? value)
    {
        var uri = navManager.ToAbsoluteUri(navManager.Uri);

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
        {
            if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
            {
                value = (T)Convert.ChangeType(valueAsInt, typeof(int));
                return true;
            }

            if (typeof(T) == typeof(string))
            {
                value = (T)Convert.ChangeType(valueFromQueryString, typeof(string));
                return true;
            }

            if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
            {
                value = (T)Convert.ChangeType(valueAsDecimal, typeof(decimal));
                return true;
            }

            if (typeof(T) == typeof(bool) && bool.TryParse(valueFromQueryString, out var valueAsBool))
            {
                value = (T)Convert.ChangeType(valueAsBool, typeof(bool));
                return true;
            }
        }

        value = default;
        return false;
    }
}
