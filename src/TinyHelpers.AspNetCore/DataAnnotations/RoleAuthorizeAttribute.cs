using Microsoft.AspNetCore.Authorization;

namespace TinyHelpers.AspNetCore.DataAnnotations;

/// <summary>
/// Builds an <see cref="AuthorizeAttribute" /> role list from individual role names.
/// </summary>
/// <remarks>
/// The attribute exists to keep authorization declarations readable when the role set is easier to express as
/// separate values than as a comma-delimited string.
/// </remarks>
public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Creates a new authorization attribute that requires one or more roles.
    /// </summary>
    /// <param name="roles">The roles a user must belong to in order to satisfy authorization.</param>
    public RoleAuthorizeAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}