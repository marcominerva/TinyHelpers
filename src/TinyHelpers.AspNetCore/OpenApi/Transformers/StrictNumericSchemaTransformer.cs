#if NET10_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

internal sealed class StrictNumericSchemaTransformer : IOpenApiSchemaTransformer
{
    /// <summary>
    /// Removes string fallbacks from numeric schemas so clients interpret the contract as strictly numeric.
    /// </summary>
    /// <param name="schema">The schema being enriched.</param>
    /// <param name="context">The schema-generation context.</param>
    /// <param name="cancellationToken">A token that signals request cancellation.</param>
    /// <returns>A task that completes when the schema has been updated.</returns>
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (schema.Type is null)
        {
            return Task.CompletedTask;
        }

        var type = schema.Type.Value;
        var hasNumeric = type.HasFlag(JsonSchemaType.Integer) || type.HasFlag(JsonSchemaType.Number);
        var hasString = type.HasFlag(JsonSchemaType.String);

        if (hasNumeric && hasString)
        {
            schema.Type &= ~JsonSchemaType.String;
            schema.Pattern = null;
        }

        return Task.CompletedTask;
    }
}

#endif