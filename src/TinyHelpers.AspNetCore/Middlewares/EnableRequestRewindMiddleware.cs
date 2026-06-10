using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.Middlewares;

internal class EnableRequestRewindMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Enables buffering on the incoming request so later components can read the body more than once.
    /// </summary>
    /// <param name="context">The current HTTP request context.</param>
    /// <remarks>
    /// This middleware exists for scenarios where an earlier component needs to inspect the body without consuming
    /// it for the rest of the pipeline, such as validation, auditing, or request-signature checks.
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        await next(context);
    }
}
