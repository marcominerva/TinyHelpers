#if NET9_0

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class OpenApiParametersOperationFilter(OpenApiOperationOptions options) : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (options.Parameters.Count > 0)
        {
            operation.Parameters ??= [];

            foreach (var parameter in options.Parameters.Where(parameter => !operation.Parameters.Any(existingParameter => existingParameter.Name == parameter.Name && existingParameter.In == parameter.In)))
            {
                operation.Parameters.Add(parameter);
            }
        }

        return Task.CompletedTask;
    }
}

#elif NET10_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class OpenApiParametersOperationFilter(OpenApiOperationOptions options) : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (options.Parameters.Count > 0)
        {
            operation.Parameters ??= [];

            foreach (var parameter in options.Parameters.Where(parameter => !operation.Parameters.Any(existingParameter => existingParameter.Name == parameter.Name && existingParameter.In == parameter.In)))
            {
                operation.Parameters.Add(parameter);
            }
        }

        return Task.CompletedTask;
    }
}

#endif