using System.ComponentModel;
using System.Security.Claims;

namespace TinyHelpers.AspNetCore.Extensions;

/// <summary>
/// Provides <see cref="ClaimsPrincipal" /> helpers for reading claim values and checking claim presence.
/// </summary>
public static class ClaimExtensions
{
    extension(ClaimsPrincipal user)
    {
        /// <summary>
        /// Returns every claim value that matches <paramref name="type" /> so multi-valued claims can be handled consistently.
        /// </summary>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The matching claim values, if any.</returns>
        /// <seealso cref="ClaimsPrincipal" />
        public IEnumerable<string?> GetClaimValues(string type)
            => user.GetClaimValues<string>(type);

        /// <summary>
        /// Returns every claim value that matches <paramref name="type" /> and converts each value to <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The CLR type to convert each claim value to.</typeparam>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The matching claim values converted to <typeparamref name="T" />.</returns>
        /// <seealso cref="ClaimsPrincipal" />
        public IEnumerable<T?> GetClaimValues<T>(string type)
        {
            var value = user.FindAll(type).Select(c => Convert<T>(c.Value)).ToList();
            return value;
        }

        /// <summary>
        /// Returns the first matching claim value so common single-value claims do not require manual enumeration.
        /// </summary>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The first matching claim value, or <see langword="null" /> when none exists.</returns>
        /// <seealso cref="ClaimsPrincipal" />
        public string? GetClaimValue(string type)
            => user.GetClaimValue<string>(type);

        /// <summary>
        /// Returns the first matching claim value and converts it to <typeparamref name="T" /> for strongly typed access.
        /// </summary>
        /// <typeparam name="T">The CLR type to convert the claim value to.</typeparam>
        /// <param name="type">The claim type to match.</param>
        /// <returns>The converted claim value, or the default value of <typeparamref name="T" /> when no claim exists.</returns>
        /// <seealso cref="ClaimsPrincipal" />
        public T? GetClaimValue<T>(string type)
        {
            var value = user.FindFirstValue(type);
            if (value is null)
            {
                return default;
            }

            return Convert<T>(value);
        }

        /// <summary>
        /// Returns whether the principal contains a claim of the requested type without exposing the underlying query.
        /// </summary>
        /// <param name="type">The claim type to match.</param>
        /// <returns><see langword="true" /> when at least one matching claim exists; otherwise, <see langword="false" />.</returns>
        /// <seealso cref="ClaimsPrincipal" />
        public bool HasClaim(string type)
        {
            var hasClaim = user.Claims.Any(c => c.Type == type);
            return hasClaim;
        }
    }

    private static T? Convert<T>(string value)
            => (T?)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
}