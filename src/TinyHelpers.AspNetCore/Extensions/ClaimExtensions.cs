using System.ComponentModel;
using System.Security.Claims;

namespace TinyHelpers.AspNetCore.Extensions;

/// <summary>
/// Provides extension methods for streamlining working with claims.
/// </summary>
public static class ClaimExtensions
{
    /// <summary>
    /// Adds or updates the claim with the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="claims">The claims list</param>
    /// <param name="type">The type of the claim to update</param>
    /// <param name="value">The value of the claim</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <seealso cref="Claim"/>
    public static void Update(this IList<Claim> claims, string type, string value)
    {
        claims.Remove(type);
        claims.Add(new Claim(type, value));
    }

    /// <summary>
    /// Removes the first occurrence of the claim with the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="claims">The claims list</param>
    /// <param name="type">The type of the claim to remove</param>
    /// <returns><see langword="true"/> if item was successfully removed; otherwise, <see langword="false"/>. This method also returns <see langword="false"/> if item is not found.</returns>
    /// <seealso cref="Claim"/>
    public static bool Remove(this IList<Claim> claims, string type)
    {
        var claim = claims.FirstOrDefault(c => c.Type == type);
        return claims.Remove(claim!);
    }

    /// <summary>
    /// Gets all the values for the claim with the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="type">The type of the claim to match.</param>
    /// <returns>The list of claim values.</returns>
    /// <seealso cref="ClaimsPrincipal"/>
    public static IEnumerable<string?> GetClaimValues(this ClaimsPrincipal user, string type)
        => user.GetClaimValues<string>(type);

    /// <summary>
    /// Gets all the values for the claim with the specified <paramref name="type"/>, casted as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The .NET type of the claim.</typeparam>
    /// <param name="user">The user.</param>
    /// <param name="type">The type of the claim to match.</param>
    /// <returns>The list of claim values.</returns>
    /// <seealso cref="ClaimsPrincipal"/>
    public static IEnumerable<T?> GetClaimValues<T>(this ClaimsPrincipal user, string type)
    {
        var value = user.FindAll(type).Select(c => Convert<T>(c.Value)).ToList();
        return value;
    }

    /// <summary>
    /// Gets the value of the first claim of the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="type">The type of the claim to match.</param>
    /// <returns>The claim value.</returns>
    /// <seealso cref="ClaimsPrincipal"/>
    public static string? GetClaimValue(this ClaimsPrincipal user, string type)
        => user.GetClaimValue<string>(type);

    /// <summary>
    /// Gets the value of the first claim of the specified <paramref name="type"/>, casted as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="user">The user.</param>
    /// <param name="type">The type of the claim to match.</param>
    /// <returns>The claim value.</returns>
    /// <seealso cref="ClaimsPrincipal"/>
    public static T? GetClaimValue<T>(this ClaimsPrincipal user, string type)
    {
        var value = user.FindFirstValue(type);
        if (value is null)
        {
            return default;
        }

        return Convert<T>(value);
    }

    /// <summary>
    /// Checks if the user has the specified claim <paramref name="type"/>.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="type">The type of the claim to match.</param>
    /// <returns><see langword="true"/> if the user has the requested claim; <see langword="false"/> otherwise.</returns>
    /// <seealso cref="ClaimsPrincipal"/>
    public static bool HasClaim(this ClaimsPrincipal user, string type)
    {
        var hasClaim = user.Claims.Any(c => c.Type == type);
        return hasClaim;
    }

    private static T? Convert<T>(string value)
        => (T?)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
}