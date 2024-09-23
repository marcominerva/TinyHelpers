using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHelpers.AspNetCore.Swagger;

internal class DefaultResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
        => operation.Responses.TryAdd("default", SwaggerExtensions.GetResponse("Error", "application/problem+json"));
}
