#if NET9_0

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

internal class DefaultResponseDocumentTransformer : IOpenApiDocumentTransformer
{
    /// <summary>
    /// Ensures the document contains a reusable <see cref="ProblemDetails" /> schema before operations reference it.
    /// </summary>
    /// <param name="document">The OpenAPI document being generated.</param>
    /// <param name="context">The document-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the schema has been added, if needed.</returns>
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        // Checks if the ProblemDetails type is already defined in the document (because there is at least one endpoint that returns it).
        var isProblemDetailsSchemaDefined = context.DescriptionGroups
           .SelectMany(g => g.Items).SelectMany(i => i.SupportedResponseTypes)
           .Any(type => type.Type == typeof(ProblemDetails));

        if (isProblemDetailsSchemaDefined)
        {
            return Task.CompletedTask;
        }

        // Otherwise, define the ProblemDetails schema in the document.
        document.Components ??= new();
        document.Components.Schemas.TryAdd(nameof(ProblemDetails), new OpenApiSchema
        {
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
        });

        return Task.CompletedTask;
    }
}

#endif