using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHelpers.AspNetCore.Swagger.Filters;

/// <summary>
/// Adds an <c>Accept-Language</c> header parameter to an OpenAPI operation when the application
/// exposes a fixed set of supported cultures.
/// </summary>
/// <remarks>
/// This keeps the generated Swagger document aligned with <see cref="RequestLocalizationOptions" />.
/// </remarks>
internal class AcceptLanguageHeaderOperationFilter(IOptions<RequestLocalizationOptions> requestLocalizationOptions) : IOperationFilter
{
    private readonly List<JsonNode>? supportedLanguages = requestLocalizationOptions.Value
            .SupportedCultures?.Select(c => JsonValue.Create(c.Name))
            .Cast<JsonNode>()
            .ToList();

    private readonly JsonNode defaultLanguage = JsonValue.Create(requestLocalizationOptions.Value.DefaultRequestCulture.Culture.Name);

    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (supportedLanguages?.Count > 0)
        {
            operation.Parameters ??= [];

            if (!operation.Parameters.Any(p => p.Name == HeaderNames.AcceptLanguage && p.In == ParameterLocation.Header))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeaderNames.AcceptLanguage,
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = JsonSchemaType.String,
                        Enum = supportedLanguages,
                        Default = defaultLanguage
                    }
                });
            }
        }
    }
}