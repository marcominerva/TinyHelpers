using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace TinyHelpers.AspNetCore.ExceptionHandlers;

internal class DefaultExceptionHandler(IProblemDetailsService problemDetailsService, IWebHostEnvironment webHostEnvironment) : IExceptionHandler
{
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