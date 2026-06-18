#if NET9_0

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class DefaultResponseOperationTransformer : IOpenApiOperationTransformer
{
    private static readonly OpenApiResponse defaultResponse = new()
    {
        Description = "Error",
        Content = new Dictionary<string, OpenApiMediaType>
        {
            [MediaTypeNames.Application.ProblemJson] = new()
            {
                Schema = new OpenApiSchema()
                {
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.Schema,
                        Id = nameof(ProblemDetails)
                    }
                }
            }
        }
    };

    /// <summary>
    /// Adds a reusable default error response so operations consistently advertise their <see cref="ProblemDetails" /> failure shape.
    /// </summary>
    /// <param name="operation">The OpenAPI operation being enriched.</param>
    /// <param name="context">The operation-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the response has been added.</returns>
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        operation.Responses.TryAdd("default", defaultResponse);
        return Task.CompletedTask;
    }
}

#elif NET10_0_OR_GREATER

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi;

/// <summary>
/// Adds a conventional default problem-details response to operations so API clients can discover a consistent error contract.
/// </summary>
public class DefaultResponseOperationTransformer : IOpenApiOperationTransformer
{
    /// <summary>
    /// Gets or sets the response key used for the fallback error response when a specific status code is not documented.
    /// </summary>
    public string DefaultResponseCode { get; set; } = "default";

    /// <summary>
    /// Gets or sets the description applied to the fallback error response so generated documents have meaningful failure metadata.
    /// </summary>
    public string DefaultDescription { get; set; } = "Error";

    /// <summary>
    /// Adds a reusable default error response so operations consistently advertise their <see cref="ProblemDetails" /> failure shape.
    /// </summary>
    /// <param name="operation">The OpenAPI operation being enriched.</param>
    /// <param name="context">The operation-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the response has been added.</returns>
    public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // Ensure ProblemDetails schema is generated.
        var problemDetailsSchema = await context.GetOrCreateSchemaAsync(typeof(ProblemDetails), cancellationToken: cancellationToken);
        context.Document?.AddComponent(nameof(ProblemDetails), problemDetailsSchema);

        // Reference the schema in responses.
        operation.Responses ??= [];
        operation.Responses.TryAdd(DefaultResponseCode, new OpenApiResponse
        {
            Description = DefaultDescription,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [MediaTypeNames.Application.ProblemJson] = new()
                {
                    Schema = new OpenApiSchemaReference(nameof(ProblemDetails), context.Document)
                }
            }
        });
    }
}

#endif