using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHelpers.AspNetCore.Swagger.Filters;

internal class AcceptLanguageHeaderOperationFilter(IOptions<RequestLocalizationOptions> requestLocalizationOptions) : IOperationFilter
{
    private readonly List<JsonNode>? supportedLanguages = requestLocalizationOptions.Value
            .SupportedCultures?.Select(c => JsonValue.Create(c.Name))
            .Cast<JsonNode>()
            .ToList();

    private readonly JsonNode defaultLanguage = JsonValue.Create(requestLocalizationOptions.Value.DefaultRequestCulture.Culture.Name);

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