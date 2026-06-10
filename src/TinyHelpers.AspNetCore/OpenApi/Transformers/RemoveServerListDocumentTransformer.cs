#if NET9_0

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class RemoveServerListDocumentTransformer : IOpenApiDocumentTransformer
{
    /// <summary>
    /// Clears the server list so the document remains portable across environments and reverse proxies.
    /// </summary>
    /// <param name="document">The OpenAPI document being generated.</param>
    /// <param name="context">The document-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the server list has been cleared.</returns>
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Servers.Clear();
        return Task.CompletedTask;
    }
}

#elif NET10_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class RemoveServerListDocumentTransformer : IOpenApiDocumentTransformer
{
    /// <summary>
    /// Clears the server list so the document remains portable across environments and reverse proxies.
    /// </summary>
    /// <param name="document">The OpenAPI document being generated.</param>
    /// <param name="context">The document-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the server list has been cleared.</returns>
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Servers?.Clear();
        return Task.CompletedTask;
    }
}

#endif