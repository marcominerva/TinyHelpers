using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHelpers.AspNetCore.Swagger;

internal class OpenApiParametersOperationFilter(OpenApiOperationOptions options) : IOperationFilter
{
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