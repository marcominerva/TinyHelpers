using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHelpers.AspNetCore.Swagger.Filters;

/// <summary>
/// Adds application-defined OpenAPI parameters to each generated operation.
/// </summary>
/// <remarks>
/// The filter centralizes shared parameters, such as custom headers or query strings, so Swagger
/// output stays consistent without repeating the same definitions on every controller action.
/// </remarks>
internal class OpenApiParametersOperationFilter(OpenApiOperationOptions options) : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (options.Parameters.Count > 0)
        {
            operation.Parameters ??= [];

            foreach (var parameter in options.Parameters.Where(parameter => !operation.Parameters.Any(existingParameter => existingParameter.Name == parameter.Name && existingParameter.In == parameter.In)))
            {
                operation.Parameters.Add(parameter);
            }
        }
    }
}