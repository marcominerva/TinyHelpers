#if NET9_0_OR_GREATER

using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class JsonNumberHandlerSchemaTransformer() : IOpenApiSchemaTransformer
{
    private const string STRING = "string";

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
            schema.Properties[property].Type = STRING;
        }

        return Task.CompletedTask;
    }
}

#endif