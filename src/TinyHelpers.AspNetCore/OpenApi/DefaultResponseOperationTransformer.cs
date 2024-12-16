#if NET9_0_OR_GREATER

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class DefaultResponseOperationTransformer : IOpenApiOperationTransformer
{
    private static readonly OpenApiSchema defaultSchema = new()
    {
        Annotations = new Dictionary<string, object>()
        {
            ["x-schema-id"] = nameof(ProblemDetails)
        },
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["type"] = new()
            {
                Type = "string",
                Nullable = true
            },
            ["title"] = new()
            {
                Type = "string",
                Nullable = true
            },
            ["status"] = new()
            {
                Type = "integer",
                Format = "int32",
                Nullable = true
            },
            ["detail"] = new()
            {
                Type = "string",
                Nullable = true
            },
            ["instance"] = new()
            {
                Type = "string",
                Nullable = true
            }
        }
    };

    private static readonly OpenApiResponse defaultResponse = new()
    {
        Description = "Error",
        Content = new Dictionary<string, OpenApiMediaType>
        {
            [MediaTypeNames.Application.ProblemJson] = new()
            {
                Schema = defaultSchema
            }
        }
    };

    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        operation.Responses.TryAdd("default", defaultResponse);
        return Task.CompletedTask;
    }
}

#endif