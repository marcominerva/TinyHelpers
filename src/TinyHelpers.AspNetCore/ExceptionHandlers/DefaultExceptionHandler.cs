using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace TinyHelpers.AspNetCore.ExceptionHandlers;

internal class DefaultExceptionHandler(IProblemDetailsService problemDetailsService, IWebHostEnvironment webHostEnvironment) : IExceptionHandler
{
    /// <summary>
    /// Converts unhandled exceptions into RFC 7807 problem details so callers receive a predictable error payload.
    /// </summary>
    /// <param name="httpContext">The current HTTP request context.</param>
    /// <param name="exception">The exception that was raised by the pipeline.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns><see langword="true" /> when the exception has been handled and written to the response.</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception is BadHttpRequestException badHttpRequestException ? badHttpRequestException.StatusCode : httpContext.Response.StatusCode;

        var problemDetails = new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = exception.GetType().FullName,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        if (exception.InnerException is not null)
        {
            problemDetails.Extensions["innerException"] = new
            {
                Title = exception.InnerException.GetType().FullName,
                Detail = exception.InnerException.Message
            };
        }

        if (webHostEnvironment.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
        }

        await problemDetailsService.WriteAsync(new()
        {
            HttpContext = httpContext,
            AdditionalMetadata = httpContext.Features.Get<IExceptionHandlerFeature>()?.Endpoint?.Metadata,
            ProblemDetails = problemDetails,
            Exception = exception
        });

        return true;
    }
}