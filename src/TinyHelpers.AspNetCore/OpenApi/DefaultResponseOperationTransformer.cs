#if NET9_0_OR_GREATER

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class DefaultResponseOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var defaultResponse = new OpenApiResponse()
        {
            Description = "Error",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [MediaTypeNames.Application.ProblemJson] = new()
                {
                    Schema = new()
                    {
                        Reference = new()
                        {
                            Id = nameof(ProblemDetails),
                            Type = ReferenceType.Schema
                        }
                    }
                }
            }
        };

        operation.Responses.TryAdd("default", defaultResponse);
        return Task.CompletedTask;
    }
}

#endif