using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using TinyHelpers.AspNetCore.Swagger.Filters;

namespace TinyHelpers.AspNetCore.Swagger;

public static class SwaggerExtensions
{
    public static void AddAcceptLanguageHeader(this SwaggerGenOptions options)
        => options.OperationFilter<AcceptLanguageHeaderOperationFilter>();

    public static void AddDefaultProblemDetailsResponse(this SwaggerGenOptions options)
        => options.OperationFilter<DefaultResponseOperationFilter>();

    public static void AddTimeSpanTypeMapping(this SwaggerGenOptions options, bool useCurrentTimeAsExample = false)
        => options.AddTimeSpanTypeMapping(useCurrentTimeAsExample ? TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm:ss") : null);

    public static void AddTimeSpanTypeMapping(this SwaggerGenOptions options, string? example)
    {
        options.MapType<TimeSpan>(() => new OpenApiSchema()
        {
            Type = JsonSchemaType.String,
            Example = example is not null ? JsonValue.Create(example) : null
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
}
