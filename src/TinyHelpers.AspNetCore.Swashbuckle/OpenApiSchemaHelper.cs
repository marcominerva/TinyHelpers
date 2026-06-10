using System.Text.Json.Nodes;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.Swagger;

/// <summary>
/// Provides factory helpers for building reusable <see cref="OpenApiSchema" /> instances.
/// </summary>
public static class OpenApiSchemaHelper
{
    /// <summary>
    /// Creates a string schema with an optional example value that can be reused in Swagger mapping.
    /// </summary>
    /// <param name="defaultValue">The default value to expose in the generated schema, if any.</param>
    /// <returns>A schema configured for <see cref="JsonSchemaType.String" />.</returns>
    public static OpenApiSchema CreateStringSchema(string? defaultValue = null)
    {
        var schema = new OpenApiSchema
        {
            Type = JsonSchemaType.String,
            Default = defaultValue is not null ? JsonValue.Create(defaultValue.ToString()) : null
        };

        return schema;
    }

    /// <summary>
    /// Creates a schema for a primitive OpenAPI type.
    /// </summary>
    /// <typeparam name="TValue">The underlying CLR type used by the schema.</typeparam>
    /// <param name="type">The OpenAPI schema type to emit.</param>
    /// <param name="format">The optional OpenAPI format name.</param>
    /// <returns>A schema configured with the supplied type and format.</returns>
    public static OpenApiSchema CreateSchema<TValue>(JsonSchemaType type, string? format = null)
    {
        var schema = new OpenApiSchema
        {
            Type = type,
            Format = format
        };

        return schema;
    }

    /// <summary>
    /// Creates a schema for a primitive OpenAPI type with a typed default value.
    /// </summary>
    /// <typeparam name="TValue">The struct type used for the default value.</typeparam>
    /// <param name="type">The OpenAPI schema type to emit.</param>
    /// <param name="format">The optional OpenAPI format name.</param>
    /// <param name="defaultValue">The default value to expose in the generated schema, if any.</param>
    /// <returns>A schema configured with the supplied type, format, and default value.</returns>
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

    /// <summary>
    /// Creates a string schema whose allowed values are limited to the supplied set.
    /// </summary>
    /// <param name="values">The values to expose in the schema enumeration.</param>
    /// <param name="defaultValue">The default value to expose in the generated schema, if any.</param>
    /// <returns>A schema configured for <see cref="JsonSchemaType.String" /> with the specified enumeration.</returns>
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

    /// <summary>
    /// Creates a string schema for an enum type by exposing all enum names as OpenAPI values.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to represent.</typeparam>
    /// <param name="defaultValue">The default enum value to expose in the generated schema, if any.</param>
    /// <returns>A schema configured for <see cref="JsonSchemaType.String" /> with all enum members.</returns>
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