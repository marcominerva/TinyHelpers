#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

internal class AcceptLanguageHeaderOperationTransformer(IOptions<RequestLocalizationOptions> requestLocalizationOptions) : IOpenApiOperationTransformer
{
    private readonly List<IOpenApiAny>? supportedLanguages = requestLocalizationOptions.Value
            .SupportedCultures?.Select(c => new OpenApiString(c.TwoLetterISOLanguageName))
            .Cast<IOpenApiAny>()
            .ToList();

    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
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

        return Task.CompletedTask;
    }
}

#endif