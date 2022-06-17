using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHelpers.AspNetCore.Swagger;

public static class SwaggerExtensions
{
    public static void AddDefaultResponse(this SwaggerGenOptions options)
    {
        options.OperationFilter<DefaultResponseOperationFilter>();
        options.DocumentFilter<ProblemDetailsDocumentFilter>();
    }

    public static void AddAcceptLanguageHeader(this SwaggerGenOptions options)
        => options.OperationFilter<AcceptLanguageHeaderOperationFilter>();

    public static void AddDateOnlyTimeOnly(this SwaggerGenOptions options)
    {
        options.MapType<DateOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date"
        });

        options.MapType<TimeOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "time",
            Example = new OpenApiString(TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"))
        });
    }

    internal static OpenApiResponse GetResponse(string description)
        => new()
        {
            Description = description,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [MediaTypeNames.Application.Json] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Id = nameof(ProblemDetails),
                            Type = ReferenceType.Schema
                        }
                    }
                }
            }
        };
}
