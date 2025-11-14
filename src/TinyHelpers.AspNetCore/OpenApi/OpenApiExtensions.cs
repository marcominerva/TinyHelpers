#if NET9_0_OR_GREATER

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
#if NET9_0
        options.AddDocumentTransformer<DefaultResponseDocumentTransformer>();
#endif
        options.AddOperationTransformer<DefaultResponseOperationTransformer>();

        return options;
    }

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

#if NET9_0
    public static OpenApiOptions EnableEnumSupport(this OpenApiOptions options)
        => options.AddSchemaTransformer<EnumSchemaTransformer>();
#endif

    /// <summary>
    /// Configures the OpenAPI schema reference IDs to use the full type name (including namespace) 
    /// instead of just the type name. This helps avoid naming collisions when multiple types have the same name.
    /// </summary>
    /// <param name="options">The <see cref="OpenApiOptions"/> to configure.</param>
    /// <returns>The <see cref="OpenApiOptions"/> instance for further customization.</returns>
    /// <remarks>
    /// By default, OpenAPI uses only the type name for schema references. This extension method changes 
    /// the behavior to use the full type name (namespace + type name) to ensure unique schema IDs.
    /// </remarks>
    public static OpenApiOptions UseFullTypeNameSchemaIds(this OpenApiOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.CreateSchemaReferenceId = (jsonTypeInfo) =>
        {
            // Get the full type name (including namespace) for the schema ID
            var fullName = jsonTypeInfo.Type.FullName;
            
            // Replace + with . for nested types to make the schema ID more readable
            return fullName?.Replace('+', '.');
        };

        return options;
    }

}

#endif