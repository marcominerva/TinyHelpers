using System.Security.Claims;

namespace TinyHelpers.AspNetCore.Extensions;

/// <summary>
/// Provides claim-collection helpers for replacing and removing items in an <see cref="IList{T}" /> of <see cref="Claim" />.
/// </summary>
public static class ClaimsExtensions
{
    extension(IList<Claim> claims)
    {
        /// <summary>
        /// Replaces the first claim that matches <paramref name="type" /> so callers can update identity state in one step.
        /// </summary>
        /// <param name="type">The claim type to replace.</param>
        /// <param name="value">The new claim value.</param>
        /// <remarks>
        /// This method removes the first matching claim before inserting the replacement so a principal does not end
        /// up with stale copies of the same logical value.
        /// </remarks>
        /// <seealso cref="Claim" />
        public void Update(string type, string value)
        {
            claims.Remove(type);
            claims.Add(new Claim(type, value));
        }

        /// <summary>
        /// Removes the first claim with the specified <paramref name="type" />.
        /// </summary>
        /// <param name="type">The claim type to remove.</param>
        /// <returns><see langword="true" /> when a claim was removed; otherwise, <see langword="false" />.</returns>
        /// <seealso cref="Claim" />
        public bool Remove(string type)
        {
            var claim = claims.FirstOrDefault(c => c.Type == type);
            return claims.Remove(claim!);
        }
    }
}