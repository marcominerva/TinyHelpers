using Microsoft.AspNetCore.Builder;

namespace TinyHelpers.AspNetCore.Middlewares;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRequestRewind(this IApplicationBuilder app)
        => app.UseMiddleware<EnableRequestRewindMiddleware>();
}
