#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

public class TimeExampleSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type == typeof(TimeSpan) || context.JsonTypeInfo.Type == typeof(TimeOnly))
        {
            schema.Example = new OpenApiString(DateTime.Now.ToString("HH:mm:ss"));
        }

        return Task.CompletedTask;
    }
}

#endif