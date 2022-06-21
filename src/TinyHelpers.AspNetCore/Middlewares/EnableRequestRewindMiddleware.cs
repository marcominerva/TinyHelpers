using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.Middlewares;

internal class EnableRequestRewindMiddleware
{
    private readonly RequestDelegate next;

    public EnableRequestRewindMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        await next(context);
    }
}
