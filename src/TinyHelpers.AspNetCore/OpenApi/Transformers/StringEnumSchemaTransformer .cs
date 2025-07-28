#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

internal class EnumAsStringTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        var isNullable = false;

        // Handle nullable enums by checking the underlying type.
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            isNullable = true;
            type = Nullable.GetUnderlyingType(type) ?? type;
        }

        if (type.IsEnum)
        {
            schema.Type = "string";

            // Remove the null value from enum list if the type was nullable.
            if (isNullable && schema.Enum is not null)
            {
                var nullValue = schema.Enum.FirstOrDefault(e => e is null || (e is OpenApiString str && str.Value is null));
                if (nullValue is not null)
                {
                    schema.Enum.Remove(nullValue);
                }
            }
        }

        return Task.CompletedTask;
    }
}

#endif