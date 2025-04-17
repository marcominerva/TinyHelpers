#if NET9_0_OR_GREATER

using System.ComponentModel;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using TinyHelpers.AspNetCore.OpenApi.Transformers;

namespace TinyHelpers.AspNetCore.OpenApi;

public static class OpenApiExtensions
{
    public static OpenApiOptions AddAcceptLanguageHeader(this OpenApiOptions options)
        => options.AddOperationTransformer<AcceptLanguageHeaderOperationTransformer>();

    public static OpenApiOptions AddDefaultProblemDetailsResponse(this OpenApiOptions options)
    {
        options.AddDocumentTransformer<DefaultResponseDocumentTransformer>();
        options.AddOperationTransformer<DefaultResponseOperationTransformer>();

        return options;
    }

    [Obsolete("Use AddDefaultProblemDetailsResponse instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static OpenApiOptions AddDefaultResponse(this OpenApiOptions options)
        => options.AddDefaultProblemDetailsResponse();

    public static IServiceCollection AddOpenApiOperationParameters(this IServiceCollection services, Action<OpenApiOperationOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        var parameters = new OpenApiOperationOptions();
        setupAction.Invoke(parameters);

        services.AddTransient(_ => parameters);

        return services;
    }

    public static OpenApiOptions AddOperationParameters(this OpenApiOptions options)
        => options.AddOperationTransformer<OpenApiParametersOperationFilter>();

    public static OpenApiOptions RemoveServerList(this OpenApiOptions options)
        => options.AddDocumentTransformer<RemoveServerListDocumentTransformer>();

    public static OpenApiOptions WriteNumberAsString(this OpenApiOptions options)
        => options.AddSchemaTransformer<WriteNumberAsStringSchemaTransformer>();

    public static OpenApiOptions DescribeAllParametersInCamelCase(this OpenApiOptions options)
        => options.AddOperationTransformer<CamelCaseQueryParametersOperationTransformer>();

    public static OpenApiOptions AddTimeExamples(this OpenApiOptions options)
        => options.AddSchemaTransformer<TimeExampleSchemaTransformer>();
}

#endif