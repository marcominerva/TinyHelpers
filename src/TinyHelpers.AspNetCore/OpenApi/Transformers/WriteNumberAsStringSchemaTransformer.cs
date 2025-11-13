#if NET9_0

using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class WriteNumberAsStringSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var properties = context.JsonTypeInfo.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(prop => prop.GetCustomAttribute<JsonNumberHandlingAttribute>()?.Handling.HasFlag(JsonNumberHandling.WriteAsString) ?? false)
                                .Select(prop => prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ??
                                                context.JsonTypeInfo.Options.PropertyNamingPolicy?.ConvertName(prop.Name) ??
                                                prop.Name)
                                .Where(schema.Properties.ContainsKey);

        foreach (var property in properties)
        {
            schema.Properties[property].Type = "string";
        }

        return Task.CompletedTask;
    }
}

#elif NET10_0_OR_GREATER

using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class WriteNumberAsStringSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var properties = context.JsonTypeInfo.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(prop => prop.GetCustomAttribute<JsonNumberHandlingAttribute>()?.Handling.HasFlag(JsonNumberHandling.WriteAsString) == true)
                                .Select(prop => prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ??
                                                context.JsonTypeInfo.Options.PropertyNamingPolicy?.ConvertName(prop.Name) ??
                                                prop.Name)
                                .Where(key => schema.Properties?.ContainsKey(key) ?? false);

        foreach (var property in properties)
        {
            var originalSchema = schema.Properties![property];

            schema.Properties![property] = new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Title = originalSchema.Title,
                Description = originalSchema.Description,
                Format = originalSchema.Format,
                ReadOnly = originalSchema.ReadOnly,
                WriteOnly = originalSchema.WriteOnly,
                Deprecated = originalSchema.Deprecated,
                Required = originalSchema.Required,
                Minimum = originalSchema.Minimum,
                Maximum = originalSchema.Maximum,
                ExclusiveMinimum = originalSchema.ExclusiveMinimum,
                ExclusiveMaximum = originalSchema.ExclusiveMaximum,
                MinLength = originalSchema.MinLength,
                MaxLength = originalSchema.MaxLength,
                Pattern = originalSchema.Pattern,
                MultipleOf = originalSchema.MultipleOf,
                Default = originalSchema.Default,
                Enum = originalSchema.Enum,
                Items = originalSchema.Items,
                AllOf = originalSchema.AllOf,
                OneOf = originalSchema.OneOf,
                AnyOf = originalSchema.AnyOf,
                Not = originalSchema.Not,
                Properties = originalSchema.Properties,
                AdditionalPropertiesAllowed = originalSchema.AdditionalPropertiesAllowed,
                AdditionalProperties = originalSchema.AdditionalProperties,
                Discriminator = originalSchema.Discriminator,
                Example = originalSchema.Example,
                ExternalDocs = originalSchema.ExternalDocs,
                Extensions = originalSchema.Extensions
            };
        }

        return Task.CompletedTask;
    }
}

#endif