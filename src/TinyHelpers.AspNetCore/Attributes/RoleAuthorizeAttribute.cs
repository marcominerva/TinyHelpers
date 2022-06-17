using Microsoft.AspNetCore.Authorization;

namespace TinyHelpers.AspNetCore.Attributes;

public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    public RoleAuthorizeAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}