#if NET9_0

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class RemoveServerListDocumentTransformer : IOpenApiDocumentTransformer
{
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
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Servers?.Clear();
        return Task.CompletedTask;
    }
}

#endif