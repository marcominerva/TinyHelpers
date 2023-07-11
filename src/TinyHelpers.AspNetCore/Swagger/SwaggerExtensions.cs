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

    //public static void AddDateOnlyTypeMapping(this SwaggerGenOptions options, bool useCurrentDateAsExample = false)
    //    => options.AddDateOnlyTypeMapping(useCurrentDateAsExample ? DateOnly.FromDateTime(DateTime.Now).ToString() : null);

    //public static void AddDateOnlyTypeMapping(this SwaggerGenOptions options, string? example)
    //{
    //    options.MapType<DateOnly>(() => new()
    //    {
    //        Type = "string",
    //        Format = "date",
    //        Example = example is not null ? new OpenApiString(example) : null
    //    });
    //}

    //public static void AddTimeOnlyTypeMapping(this SwaggerGenOptions options, bool useCurrentTimeAsExample = false)
    //    => options.AddTimeOnlyTypeMapping(useCurrentTimeAsExample ? TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm:ss") : null);

    //public static void AddTimeOnlyTypeMapping(this SwaggerGenOptions options, string? example)
    //{
    //    options.MapType<TimeOnly>(() => new()
    //    {
    //        Type = "string",
    //        Format = "time",
    //        Example = example is not null ? new OpenApiString(example) : null
    //    });
    //}

    public static void AddTimeSpanTypeMapping(this SwaggerGenOptions options, bool useCurrentTimeAsExample = false)
        => options.AddTimeSpanTypeMapping(useCurrentTimeAsExample ? TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm:ss") : null);

    public static void AddTimeSpanTypeMapping(this SwaggerGenOptions options, string? example)
    {
        options.MapType<TimeSpan>(() => new()
        {
            Type = "string",
            Example = example is not null ? new OpenApiString(example) : null
        });
    }

    public static IServiceCollection AddSwaggerOperationParameters(this IServiceCollection services, Action<OpenApiOperationOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        var parameters = new OpenApiOperationOptions();
        setupAction.Invoke(parameters);

        services.AddTransient(_ => parameters);

        return services;
    }

    public static void AddOperationParameters(this SwaggerGenOptions options)
        => options.OperationFilter<OpenApiParametersOperationFilter>();

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
