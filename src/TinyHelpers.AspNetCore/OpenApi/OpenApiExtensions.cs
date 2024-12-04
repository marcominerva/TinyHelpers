#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace TinyHelpers.AspNetCore.OpenApi;

public static class OpenApiExtensions
{
    public static void AddAcceptLanguageHeader(this OpenApiOptions options)
        => options.AddOperationTransformer<AcceptLanguageHeaderOperationTransformer>();

    public static void AddDefaultResponse(this OpenApiOptions options)
        => options.AddOperationTransformer<DefaultResponseOperationTransformer>();

    public static IServiceCollection AddOpenApiOperationParameters(this IServiceCollection services, Action<OpenApiOperationOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        var parameters = new OpenApiOperationOptions();
        setupAction.Invoke(parameters);

        services.AddTransient(_ => parameters);

        return services;
    }

    public static void AddOperationParameters(this OpenApiOptions options)
        => options.AddOperationTransformer<OpenApiParametersOperationFilter>();
}

#endif