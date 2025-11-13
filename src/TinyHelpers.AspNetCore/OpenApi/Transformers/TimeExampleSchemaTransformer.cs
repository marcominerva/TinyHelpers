#if NET9_0

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

public class TimeExampleSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        if (type == typeof(TimeSpan) || type == typeof(TimeSpan?) || type == typeof(TimeOnly) || type == typeof(TimeOnly?))
        {
            schema.Example = new OpenApiString(DateTime.Now.ToString("HH:mm:ss"));
        }

        return Task.CompletedTask;
    }
}

#elif NET10_0_OR_GREATER

using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

public class TimeExampleSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        if (type == typeof(TimeSpan) || type == typeof(TimeSpan?) || type == typeof(TimeOnly) || type == typeof(TimeOnly?))
        {
            schema.Example = JsonValue.Create(DateTime.Now.ToString("HH:mm:ss"));
        }

        return Task.CompletedTask;
    }
}

#endif