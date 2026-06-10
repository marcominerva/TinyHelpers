#if NET9_0

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class OpenApiParametersOperationFilter(OpenApiOperationOptions options) : IOpenApiOperationTransformer
{
    /// <summary>
    /// Copies custom parameter metadata into an operation so shared rules can be registered once and reused across endpoints.
    /// </summary>
    /// <param name="operation">The OpenAPI operation being enriched.</param>
    /// <param name="context">The operation-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the parameters have been copied.</returns>
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
    /// <summary>
    /// Copies custom parameter metadata into an operation so shared rules can be registered once and reused across endpoints.
    /// </summary>
    /// <param name="operation">The OpenAPI operation being enriched.</param>
    /// <param name="context">The operation-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the parameters have been copied.</returns>
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