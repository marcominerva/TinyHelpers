using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHelpers.AspNetCore.Swagger;

internal class AcceptLanguageHeaderOperationFilter(IOptions<RequestLocalizationOptions> requestLocalizationOptions) : IOperationFilter
{
    private readonly List<IOpenApiAny>? supportedLanguages = requestLocalizationOptions.Value
            .SupportedCultures?.Select(c => new OpenApiString(c.TwoLetterISOLanguageName))
            .Cast<IOpenApiAny>()
            .ToList();

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (supportedLanguages?.Count > 0)
        {
            operation.Parameters ??= [];

            if (!operation.Parameters.Any(p => p.Name == HeaderNames.AcceptLanguage && p.In == ParameterLocation.Header))
            {
                operation.Parameters.Add(new()
                {
                    Name = HeaderNames.AcceptLanguage,
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = supportedLanguages,
                        Default = supportedLanguages.First()
                    }
                });
            }
        }
    }
}