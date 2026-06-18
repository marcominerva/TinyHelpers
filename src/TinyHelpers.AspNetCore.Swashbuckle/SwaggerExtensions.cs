using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using TinyHelpers.AspNetCore.Swagger.Filters;

namespace TinyHelpers.AspNetCore.Swagger;

/// <summary>
/// Adds reusable Swagger and OpenAPI conventions for <see cref="SwaggerGenOptions" />.
/// </summary>
public static class SwaggerExtensions
{
    extension(SwaggerGenOptions options)
    {
        /// <summary>
        /// Documents the <c>Accept-Language</c> header for operations that support localization.
        /// </summary>
        public void AddAcceptLanguageHeader()
            => options.OperationFilter<AcceptLanguageHeaderOperationFilter>();

        /// <summary>
        /// Adds a shared <c>application/problem+json</c> response to generated operations.
        /// </summary>
        public void AddDefaultProblemDetailsResponse()
            => options.OperationFilter<DefaultResponseOperationFilter>();

        /// <summary>
        /// Maps <see cref="TimeSpan" /> to an OpenAPI string schema with an optional example value.
        /// </summary>
        /// <param name="useCurrentTimeAsExample">
        /// When <see langword="true" />, uses the current time as the example value to make the schema
        /// easier to interpret in Swagger UI.
        /// </param>
        public void AddTimeSpanTypeMapping(bool useCurrentTimeAsExample = false)
            => options.AddTimeSpanTypeMapping(useCurrentTimeAsExample ? TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm:ss") : null);

        /// <summary>
        /// Maps <see cref="TimeSpan" /> to an OpenAPI string schema using the supplied example value.
        /// </summary>
        /// <param name="example">The example value to expose in the generated OpenAPI schema.</param>
        public void AddTimeSpanTypeMapping(string? example)
        {
            options.MapType<TimeSpan>(() => new OpenApiSchema()
            {
                Type = JsonSchemaType.String,
                Example = example is not null ? JsonValue.Create(example) : null
            });
        }

        /// <summary>
        /// Adds the operation filter that copies registered shared parameters into each generated Swagger operation.
        /// </summary>
        /// <remarks>
        /// Use this in Swagger configuration after registering parameters with
        /// <see cref="AddSwaggerOperationParameters(IServiceCollection, Action{OpenApiOperationOptions})" />. Keeping
        /// parameter definitions in dependency injection and applying them through a filter prevents duplicated route
        /// metadata while preserving a complete contract for generated clients.
        /// </remarks>
        /// <seealso cref="AddSwaggerOperationParameters(IServiceCollection, Action{OpenApiOperationOptions})"/>
        public void AddOperationParameters()
            => options.OperationFilter<OpenApiParametersOperationFilter>();
    }

    /// <summary>
    /// Registers shared Swagger operation parameters that can later be merged into generated operations.
    /// </summary>
    /// <param name="services">The service collection to extend.</param>
    /// <param name="setupAction">
    /// A callback that adds reusable parameter definitions to an <see cref="OpenApiOperationOptions" /> instance.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection" /> instance so calls can be chained.</returns>
    /// <remarks>
    /// Call this during service registration when a parameter, such as a tenant, correlation, or feature header,
    /// must appear consistently in the Swagger contract without being repeated on every endpoint. The registered
    /// options are consumed by <see cref="AddOperationParameters(SwaggerGenOptions)" /> when the document is generated.
    /// </remarks>
    /// <seealso cref="AddOperationParameters(SwaggerGenOptions)"/>
    public static IServiceCollection AddSwaggerOperationParameters(this IServiceCollection services, Action<OpenApiOperationOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        var parameters = new OpenApiOperationOptions();
        setupAction.Invoke(parameters);

        services.AddTransient(_ => parameters);

        return services;
    }
}
