#if NET9_0

using System.Globalization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

/// <summary>
/// Creates reusable schema fragments so OpenAPI transformers can express the same contract details without duplicating schema setup.
/// </summary>
/// <remarks>
/// These factory methods keep schema construction consistent across transformers that need to describe default values,
/// formats, and enum choices for generated clients.
/// </remarks>
public static class OpenApiSchemaHelper
{
    /// <summary>
    /// Creates a string schema with an optional default value for reusable text-based contract metadata.
    /// </summary>
    /// <param name="defaultValue">The optional default value.</param>
    /// <returns>A schema configured for the OpenAPI string type.</returns>
    public static OpenApiSchema CreateStringSchema(string? defaultValue = null)
    {
        var schema = new OpenApiSchema
        {
            Type = "string",
            Default = defaultValue is not null ? new OpenApiString(defaultValue) : null
        };

        return schema;
    }

    /// <summary>
    /// Creates a typed schema with an optional format when a transformer needs to describe a primitive OpenAPI shape.
    /// </summary>
    /// <typeparam name="TValue">The type associated with the schema.</typeparam>
    /// <param name="type">The OpenAPI type name.</param>
    /// <param name="format">The optional format string.</param>
    /// <returns>A schema configured with the supplied type metadata.</returns>
    public static OpenApiSchema CreateSchema<TValue>(string type, string? format = null)
    {
        var schema = new OpenApiSchema
        {
            Type = type,
            Format = format
        };

        return schema;
    }

    /// <summary>
    /// Creates a typed schema with a default value so generated clients can display the same fallback used by the API contract.
    /// </summary>
    /// <typeparam name="TValue">The value type associated with the schema.</typeparam>
    /// <param name="type">The OpenAPI type name.</param>
    /// <param name="format">The optional format string.</param>
    /// <param name="defaultValue">The optional default value.</param>
    /// <returns>A schema configured with the supplied type metadata and default.</returns>
    public static OpenApiSchema CreateSchema<TValue>(string type, string? format, TValue? defaultValue = null) where TValue : struct
    {
        var schema = new OpenApiSchema
        {
            Type = type,
            Format = format,
            Default = defaultValue is not null ? new OpenApiString(Convert.ToString(defaultValue, CultureInfo.InvariantCulture)) : null
        };

        return schema;
    }

    /// <summary>
    /// Creates a string enum schema from a known list of values when the valid choices are defined outside a CLR enum.
    /// </summary>
    /// <param name="values">The allowed values to expose in the schema.</param>
    /// <param name="defaultValue">The optional default value.</param>
    /// <returns>A string schema with the supplied enum values.</returns>
    public static OpenApiSchema CreateSchema(IEnumerable<string> values, string? defaultValue = null)
    {
        var schema = new OpenApiSchema
        {
            Type = "string",
            Enum = values.Select(v => new OpenApiString(v)).Cast<IOpenApiAny>().ToList(),
            Default = defaultValue is not null ? new OpenApiString(defaultValue) : null
        };

        return schema;
    }

    /// <summary>
    /// Creates an enum schema from an <see cref="Enum"/> type so the generated document exposes every declared value.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type to describe.</typeparam>
    /// <param name="defaultValue">The optional default enum value.</param>
    /// <returns>A string enum schema populated with every declared member.</returns>
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

#elif NET10_0_OR_GREATER

using System.Globalization;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi;

/// <summary>
/// Creates reusable schema fragments so OpenAPI transformers can express the same contract details without duplicating schema setup.
/// </summary>
/// <remarks>
/// These factory methods keep schema construction consistent across transformers that need to describe default values,
/// formats, and enum choices for generated clients.
/// </remarks>
public static class OpenApiSchemaHelper
{
    /// <summary>
    /// Creates a string schema with an optional default value for reusable text-based contract metadata.
    /// </summary>
    /// <param name="defaultValue">The optional default value.</param>
    /// <returns>A schema configured for <see cref="JsonSchemaType.String" />.</returns>
    public static OpenApiSchema CreateStringSchema(string? defaultValue = null)
    {
        var schema = new OpenApiSchema
        {
            Type = JsonSchemaType.String,
            Default = defaultValue is not null ? JsonValue.Create(defaultValue) : null
        };

        return schema;
    }

    /// <summary>
    /// Creates a typed schema with an optional format when a transformer needs to describe a primitive OpenAPI shape.
    /// </summary>
    /// <typeparam name="TValue">The type associated with the schema.</typeparam>
    /// <param name="type">The OpenAPI type value.</param>
    /// <param name="format">The optional format string.</param>
    /// <returns>A schema configured with the supplied type metadata.</returns>
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
    /// Creates a typed schema with a default value so generated clients can display the same fallback used by the API contract.
    /// </summary>
    /// <typeparam name="TValue">The type associated with the schema.</typeparam>
    /// <param name="type">The OpenAPI type value.</param>
    /// <param name="format">The optional format string.</param>
    /// <param name="defaultValue">The optional default value.</param>
    /// <returns>A schema configured with the supplied type metadata and default.</returns>
    public static OpenApiSchema CreateSchema<TValue>(JsonSchemaType type, string? format, TValue? defaultValue = null) where TValue : struct
    {
        var schema = new OpenApiSchema
        {
            Type = type,
            Format = format,
            Default = defaultValue is not null ? JsonValue.Create(Convert.ToString(defaultValue, CultureInfo.InvariantCulture)) : null
        };

        return schema;
    }

    /// <summary>
    /// Creates a string enum schema from a known list of values when the valid choices are defined outside a CLR enum.
    /// </summary>
    /// <param name="values">The allowed values to expose in the schema.</param>
    /// <param name="defaultValue">The optional default value.</param>
    /// <returns>A string schema with the supplied enum values.</returns>
    public static OpenApiSchema CreateSchema(IEnumerable<string> values, string? defaultValue = null)
    {
        var schema = new OpenApiSchema
        {
            Type = JsonSchemaType.String,
            Enum = values.Select(v => JsonValue.Create(v)).Cast<JsonNode>().ToList(),
            Default = defaultValue is not null ? JsonValue.Create(defaultValue) : null
        };

        return schema;
    }

    /// <summary>
    /// Creates an enum schema from an <see cref="Enum"/> type so the generated document exposes every declared value.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type to describe.</typeparam>
    /// <param name="defaultValue">The optional default enum value.</param>
    /// <returns>A string enum schema populated with every declared member.</returns>
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

#endif