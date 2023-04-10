using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace TinyHelpers.AspNetCore.Extensions;

public static class NavigationManagerExtensions
{
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
