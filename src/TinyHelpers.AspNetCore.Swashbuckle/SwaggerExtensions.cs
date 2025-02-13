using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;
using TinyHelpers.AspNetCore.Swagger.Filters;

namespace TinyHelpers.AspNetCore.Swagger;

public static class SwaggerExtensions
{
    public static void AddAcceptLanguageHeader(this SwaggerGenOptions options)
        => options.OperationFilter<AcceptLanguageHeaderOperationFilter>();

    public static void AddDefaultProblemDetailsResponse(this SwaggerGenOptions options)
    {
        options.DocumentFilter<ProblemDetailsDocumentFilter>();
        options.OperationFilter<DefaultResponseOperationFilter>();
    }

    [Obsolete("Use AddDefaultProblemDetailsResponse instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void AddDefaultResponse(this SwaggerGenOptions options)
        => options.AddDefaultProblemDetailsResponse();

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
}
