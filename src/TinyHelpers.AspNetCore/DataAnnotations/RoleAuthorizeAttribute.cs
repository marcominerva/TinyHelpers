using Microsoft.AspNetCore.Authorization;

namespace TinyHelpers.AspNetCore.DataAnnotations;

public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    public RoleAuthorizeAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}