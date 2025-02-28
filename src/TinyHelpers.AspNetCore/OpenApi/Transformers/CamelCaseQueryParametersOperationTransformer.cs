#if NET9_0_OR_GREATER

using System.Text.Json;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

public class CamelCaseQueryParametersOperationTransformer : IOpenApiOperationTransformer
{
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

#endif