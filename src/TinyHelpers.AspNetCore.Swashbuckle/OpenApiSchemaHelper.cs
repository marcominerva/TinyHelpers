using System.Text.Json.Nodes;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.Swagger;

public static class OpenApiSchemaHelper
{
    public static OpenApiSchema CreateStringSchema(string? defaultValue = null)
    {
        var schema = new OpenApiSchema
        {
            Type = JsonSchemaType.String,
            Default = defaultValue is not null ? JsonValue.Create(defaultValue.ToString()) : null
        };

        return schema;
    }

    public static OpenApiSchema CreateSchema<TValue>(JsonSchemaType type, string? format = null)
    {
        var schema = new OpenApiSchema
        {
            Type = type,
            Format = format
        };

        return schema;
    }

    public static OpenApiSchema CreateSchema<TValue>(JsonSchemaType type, string? format, TValue? defaultValue = null) where TValue : struct
    {
        var schema = new OpenApiSchema
        {
            Type = type,
            Format = format,
            Default = defaultValue is not null ? JsonValue.Create(defaultValue.ToString()) : null
        };

        return schema;
    }

    public static OpenApiSchema CreateSchema(IEnumerable<string> values, string? defaultValue = null)
    {
        var schema = new OpenApiSchema
        {
            Type = JsonSchemaType.String,
            Enum = values.Select(v => JsonValue.Create(v)).Cast<JsonNode>().ToList(),
            Default = defaultValue is not null ? JsonValue.Create(defaultValue.ToString()) : null
        };

        return schema;
    }

    public static OpenApiSchema CreateSchema<TEnum>(TEnum? defaultValue = null) where TEnum : struct, Enum
    {
        var schema = new OpenApiSchema
        {
            Type = JsonSchemaType.String,
            Enum = Enum.GetValues<TEnum>().Select(e => JsonValue.Create(e.ToString())).Cast<JsonNode>().ToList(),
            Default = defaultValue.HasValue ? JsonValue.Create(defaultValue.ToString()) : null
        };

        return schema;
    }
}