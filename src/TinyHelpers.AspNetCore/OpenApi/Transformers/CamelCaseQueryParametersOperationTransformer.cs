#if NET9_0

using System.Text.Json;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

public class CamelCaseQueryParametersOperationTransformer : IOpenApiOperationTransformer
{
    /// <summary>
    /// Renames query parameters to camel case so generated clients see the same casing contract the API expects at runtime.
    /// </summary>
    /// <param name="operation">The OpenAPI operation being enriched.</param>
    /// <param name="context">The operation-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the document has been updated.</returns>
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // Ensures that query parameters are camel-cased in the OpenAPI document.
        if (operation.Parameters is not null)
        {
            foreach (var parameter in operation.Parameters.Where(p => p.In is ParameterLocation.Query))
            {
                parameter.Name = JsonNamingPolicy.CamelCase.ConvertName(parameter.Name);
            }
        }

        return Task.CompletedTask;
    }
}

#elif NET10_0_OR_GREATER

using System.Text.Json;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

public class CamelCaseQueryParametersOperationTransformer : IOpenApiOperationTransformer
{
    /// <summary>
    /// Renames query parameters to camel case so generated clients see the same casing contract the API expects at runtime.
    /// </summary>
    /// <param name="operation">The OpenAPI operation being enriched.</param>
    /// <param name="context">The operation-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the document has been updated.</returns>
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // Ensures that query parameters are camel-cased in the OpenAPI document.
        if (operation.Parameters is not null)
        {
            for (var i = 0; i < operation.Parameters.Count; i++)
            {
                var parameter = operation.Parameters[i];
                if (parameter.In == ParameterLocation.Query)
                {
                    var camelCaseName = JsonNamingPolicy.CamelCase.ConvertName(parameter.Name!);
                    if (parameter.Name != camelCaseName)
                    {
                        operation.Parameters[i] = new OpenApiParameter
                        {
                            Name = camelCaseName,
                            In = parameter.In,
                            Description = parameter.Description,
                            Required = parameter.Required,
                            Deprecated = parameter.Deprecated,
                            AllowEmptyValue = parameter.AllowEmptyValue,
                            Style = parameter.Style,
                            Explode = parameter.Explode,
                            AllowReserved = parameter.AllowReserved,
                            Schema = parameter.Schema,
                            Example = parameter.Example,
                            Examples = parameter.Examples,
                            Content = parameter.Content,
                            Extensions = parameter.Extensions
                        };
                    }
                }
            }
        }

        return Task.CompletedTask;
    }
}

#endif