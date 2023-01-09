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

    public static void AddDateOnlyTypeMapping(this SwaggerGenOptions options, bool useCurrentDateAsExample = false)
        => options.AddDateOnlyTypeMapping(useCurrentDateAsExample ? DateOnly.FromDateTime(DateTime.Now).ToString() : null);

    public static void AddDateOnlyTypeMapping(this SwaggerGenOptions options, string? example)
    {
        options.MapType<DateOnly>(() => new()
        {
            Type = "string",
            Format = "date",
            Example = example is not null ? new OpenApiString(example) : null
        });
    }

    public static void AddTimeOnlyTypeMapping(this SwaggerGenOptions options, bool useCurrentTimeAsExample = false)
        => options.AddTimeOnlyTypeMapping(useCurrentTimeAsExample ? TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss") : null);

    public static void AddTimeOnlyTypeMapping(this SwaggerGenOptions options, string? example)
    {
        options.MapType<TimeOnly>(() => new()
        {
            Type = "string",
            Format = "time",
            Example = example is not null ? new OpenApiString(example) : null
        });
    }

    public static void AddDateOnlyTimeOnlyTypeMappings(this SwaggerGenOptions options, bool useCurrentDateAsExample = false, bool useCurrentTimeAsExample = false)
    {
        options.AddDateOnlyTypeMapping(useCurrentDateAsExample);
        options.AddTimeOnlyTypeMapping(useCurrentTimeAsExample);
    }

    public static void AddDateOnlyTimeOnlyTypeMappings(this SwaggerGenOptions options, string? dateExample, string? timeExample)
    {
        options.AddDateOnlyTypeMapping(dateExample);
        options.AddTimeOnlyTypeMapping(timeExample);
    }

    internal static OpenApiResponse GetResponse(string description)
        => new()
        {
            Description = description,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [MediaTypeNames.Application.Json] = new()
                {
                    Schema = new()
                    {
                        Reference = new()
                        {
                            Id = nameof(ProblemDetails),
                            Type = ReferenceType.Schema
                        }
                    }
                }
            }
        };
}
