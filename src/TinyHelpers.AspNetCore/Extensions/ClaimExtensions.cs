using System.ComponentModel;
using System.Security.Claims;
using System.Security.Principal;

namespace TinyHelpers.AspNetCore.Extensions;

public static class ClaimExtensions
{
    public static IEnumerable<string?> GetRoles(this IPrincipal user)
        => user.GetClaimValues<string?>(ClaimTypes.Role);

    public static IEnumerable<T?> GetClaimValues<T>(this IPrincipal user, string type)
    {
        var value = ((ClaimsPrincipal)user).FindAll(type).Select(c => c.Value).Cast<T>().ToList();
        return value;
    }

    public static T? GetClaimValue<T>(this IPrincipal user, string type)
    {
        var value = ((ClaimsPrincipal)user).FindFirstValue(type);
        if (value is null)
        {
            return default;
        }

        return (T?)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
    }

    public static bool HasClaim(this IPrincipal user, string type)
    {
        var hasClaim = ((ClaimsPrincipal)user).Claims.Any(c => c.Type == type);
        return hasClaim;
    }
}