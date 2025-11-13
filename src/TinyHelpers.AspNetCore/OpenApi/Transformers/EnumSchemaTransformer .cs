#if NET9_0

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi.Transformers;

internal class EnumSchemaTransformer : IOpenApiSchemaTransformer
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
            if (schema.Type == "integer")
            {
                // Gets integer enum values.
                var values = Enum.GetValues(type);
                var enumList = new List<IOpenApiAny>(values.Length + (isNullable ? 1 : 0));

                foreach (var value in values)
                {
                    enumList.Add(new OpenApiInteger((int)value));
                }

                if (isNullable)
                {
                    // If the enum is nullable, add a null value to the enum list.
                    enumList.Add(new OpenApiNull());
                }

                schema.Enum = enumList;
            }
            else
            {
                // If the enum is not an integer type, we treat it as a string.
                schema.Type = "string";
            }
        }

        return Task.CompletedTask;
    }
}

#endif