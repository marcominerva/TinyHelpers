#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class OpenApiParametersOperationFilter(OpenApiOperationOptions options) : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (options?.Parameters.Count > 0)
        {
            operation.Parameters ??= [];

            foreach (var parameter in options.Parameters.Where(parameter => !operation.Parameters.Any(existingParameter => existingParameter.Name == parameter.Name && existingParameter.In == parameter.In)))
            {
                operation.Parameters.Add(parameter);
            }
        }

        return Task.CompletedTask;
    }
}

public class OpenApiOperationOptions
{
    internal OpenApiOperationOptions()
    {
    }

    public IList<OpenApiParameter> Parameters { get; init; } = [];
}

public static class OpenApiSchemaHelper
{
    public static OpenApiSchema CreateStringSchema(string? defaultValue = null)
    {
        var schema = new OpenApiSchema
        {
            Type = "string",
            Default = defaultValue is not null ? new OpenApiString(defaultValue.ToString()) : null
        };

        return schema;
    }

    public static OpenApiSchema CreateSchema<TValue>(string type, string? format = null)
    {
        var schema = new OpenApiSchema
        {
            Type = type,
            Format = format
        };

        return schema;
    }

    public static OpenApiSchema CreateSchema<TValue>(string type, string? format, TValue? defaultValue = null) where TValue : struct
    {
        var schema = new OpenApiSchema
        {
            Type = type,
            Format = format,
            Default = defaultValue is not null ? new OpenApiString(defaultValue.ToString()) : null
        };

        return schema;
    }

    public static OpenApiSchema CreateSchema(IEnumerable<string> values, string? defaultValue = null)
    {
        var schema = new OpenApiSchema
        {
            Type = "string",
            Enum = values.Select(v => new OpenApiString(v)).Cast<IOpenApiAny>().ToList(),
            Default = defaultValue is not null ? new OpenApiString(defaultValue.ToString()) : null
        };

        return schema;
    }

    public static OpenApiSchema CreateSchema<TEnum>(TEnum? defaultValue = null) where TEnum : struct, Enum
    {
        var schema = new OpenApiSchema
        {
            Type = "string",
            Enum = Enum.GetValues<TEnum>().Select(e => new OpenApiString(e.ToString())).Cast<IOpenApiAny>().ToList(),
            Default = defaultValue.HasValue ? new OpenApiString(defaultValue.ToString()) : null
        };

        return schema;
    }
}

#endif