using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.Middlewares;

internal class EnableRequestRewindMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        await next(context);
    }
}
