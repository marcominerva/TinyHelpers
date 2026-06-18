using Microsoft.AspNetCore.Builder;

namespace TinyHelpers.AspNetCore.Middlewares;

/// <summary>
/// Provides middleware registration helpers for request-body behaviors that are otherwise easy to forget in pipeline setup.
/// </summary>
public static class ApplicationBuilderExtensions
{
    extension(IApplicationBuilder app)
    {
        /// <summary>
        /// Enables request buffering so downstream middleware and services can re-read the body after an earlier component has inspected it.
        /// </summary>
        /// <returns>The <see cref="IApplicationBuilder" /> builder instance.</returns>
        public IApplicationBuilder UseRequestRewind()
            => app.UseMiddleware<EnableRequestRewindMiddleware>();
    }
}
