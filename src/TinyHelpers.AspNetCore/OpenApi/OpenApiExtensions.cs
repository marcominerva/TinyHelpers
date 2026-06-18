#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TinyHelpers.AspNetCore.OpenApi.Transformers;

namespace TinyHelpers.AspNetCore.OpenApi;

/// <summary>
/// Adds opinionated <see cref="OpenApiOptions" /> and service-registration helpers used to keep generated OpenAPI
/// documents aligned with the runtime behavior configured by this library.
/// </summary>
/// <remarks>
/// These helpers centralize reusable OpenAPI conventions, such as shared operation parameters and default error
/// responses, so applications do not need to repeat the same transformer setup for every document or endpoint.
/// </remarks>
public static class OpenApiExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers shared OpenAPI operation parameters that can later be merged into generated operations.
        /// </summary>
        /// <param name="setupAction">
        /// A callback that adds reusable parameter definitions to an <see cref="OpenApiOperationOptions" /> instance.
        /// </param>
        /// <returns>The same <see cref="IServiceCollection" /> instance so registrations can continue fluently.</returns>
        /// <remarks>
        /// Call this during service registration when a parameter, such as a tenant, correlation, or feature header,
        /// must appear consistently in the OpenAPI contract without being repeated on every endpoint. The registered
        /// options are consumed by <see cref="AddOperationParameters(OpenApiOptions)" /> when the OpenAPI document is generated.
        /// </remarks>
        /// <seealso cref="AddOperationParameters(OpenApiOptions)"/>
        public IServiceCollection AddOpenApiOperationParameters(Action<OpenApiOperationOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(setupAction);

            var parameters = new OpenApiOperationOptions();
            setupAction.Invoke(parameters);

            services.AddTransient(_ => parameters);

            return services;
        }
    }

    extension(OpenApiOptions options)
    {
        /// <summary>
        /// Adds an <c>Accept-Language</c> header parameter when localization is available so the generated contract matches the runtime behavior.
        /// </summary>
        /// <returns>The same <see cref="OpenApiOptions" /> for fluent configuration.</returns>
        public OpenApiOptions AddAcceptLanguageHeader()
            => options.AddOperationTransformer<AcceptLanguageHeaderOperationTransformer>();

        /// <summary>
        /// Ensures operations advertise a default <see cref="ProblemDetails" /> response so API consumers can rely on a consistent error shape.
        /// </summary>
        /// <returns>The same <see cref="OpenApiOptions" /> for fluent configuration.</returns>
        public OpenApiOptions AddDefaultProblemDetailsResponse()
        {
#if NET9_0
            options.AddDocumentTransformer<DefaultResponseDocumentTransformer>();
#endif
            options.AddOperationTransformer<DefaultResponseOperationTransformer>();

            return options;
        }

        /// <summary>
        /// Adds the operation transformer that copies registered shared parameters into each generated OpenAPI operation.
        /// </summary>
        /// <remarks>
        /// Use this in the OpenAPI document configuration after registering parameters with
        /// <see cref="AddOpenApiOperationParameters(IServiceCollection, Action{OpenApiOperationOptions})" />. Keeping
        /// parameter definitions in dependency injection and applying them through a transformer prevents duplicated
        /// route metadata while preserving a complete contract for generated clients.
        /// </remarks>
        /// <returns>The same <see cref="OpenApiOptions" /> instance so OpenAPI configuration can continue fluently.</returns>
        /// <seealso cref="AddOpenApiOperationParameters(IServiceCollection, Action{OpenApiOperationOptions})"/>
        public OpenApiOptions AddOperationParameters()
            => options.AddOperationTransformer<OpenApiParametersOperationFilter>();

        /// <summary>
        /// Removes the server list from the document when the deployment URL should be inferred from the current host instead of being hard-coded.
        /// </summary>
        /// <returns>The same <see cref="OpenApiOptions" /> for fluent configuration.</returns>
        public OpenApiOptions RemoveServerList()
            => options.AddDocumentTransformer<RemoveServerListDocumentTransformer>();

        /// <summary>
        /// Serializes numeric values as strings in schema metadata when the contract must mirror a text-based transport or client expectation.
        /// </summary>
        /// <returns>The same <see cref="OpenApiOptions" /> for fluent configuration.</returns>
        public OpenApiOptions WriteNumberAsString()
            => options.AddSchemaTransformer<WriteNumberAsStringSchemaTransformer>();

        /// <summary>
        /// Normalizes query-parameter names to camel case so the generated document matches the JSON naming strategy used by most client code.
        /// </summary>
        /// <returns>The same <see cref="OpenApiOptions" /> for fluent configuration.</returns>
        public OpenApiOptions DescribeAllParametersInCamelCase()
            => options.AddOperationTransformer<CamelCaseQueryParametersOperationTransformer>();

        /// <summary>
        /// Adds representative time examples to schemas so consumers can understand the expected shape without reading implementation code.
        /// </summary>
        /// <returns>The same <see cref="OpenApiOptions" /> for fluent configuration.</returns>
        public OpenApiOptions AddTimeExamples()
            => options.AddSchemaTransformer<TimeExampleSchemaTransformer>();

#if NET9_0
        /// <summary>
        /// Expands enum schemas so generated clients can choose the documented values rather than inferring them from CLR metadata.
        /// </summary>
        /// <returns>The same <see cref="OpenApiOptions" /> for fluent configuration.</returns>
        public OpenApiOptions EnableEnumSupport()
            => options.AddSchemaTransformer<EnumSchemaTransformer>();
#endif

        /// <summary>
        /// Configures schema reference IDs to include the namespace so types with the same name do not collide in large models.
        /// </summary>
        /// <returns>The same <see cref="OpenApiOptions" /> instance for further customization.</returns>
        /// <remarks>
        /// OpenAPI defaults to the short type name, which is easy to read but can produce duplicate schema IDs when a
        /// project has multiple types with the same name. Including the namespace keeps the document stable and unambiguous.
        /// </remarks>
        public OpenApiOptions UseFullTypeNameSchemaIds()
        {
            ArgumentNullException.ThrowIfNull(options);

            options.CreateSchemaReferenceId = jsonTypeInfo =>
            {
                // Create the default ID (handles generics, nested types, etc.)
                var defaultId = OpenApiOptions.CreateDefaultSchemaReferenceId(jsonTypeInfo);
                if (string.IsNullOrEmpty(defaultId))
                {
                    return defaultId;
                }

                var @namespace = jsonTypeInfo.Type.Namespace;
                if (string.IsNullOrEmpty(@namespace))
                {
                    // If there's no namespace, just keep the default.
                    return defaultId;
                }

                // Include namespace in the reference ID.
                return $"{@namespace}.{defaultId}";
            };

            return options;
        }

#if NET10_0_OR_GREATER
        /// <summary>
        /// Forces numeric schemas to stay numeric so generated documents can match stricter OpenAPI client expectations.
        /// </summary>
        /// <returns>The same <see cref="OpenApiOptions" /> for fluent configuration.</returns>
        public OpenApiOptions UseStrictNumericSchemas()
            => options.AddSchemaTransformer<StrictNumericSchemaTransformer>();
#endif
    }
}

#endif