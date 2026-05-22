#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using TinyHelpers.AspNetCore.OpenApi.Transformers;

namespace TinyHelpers.AspNetCore.OpenApi;

public static class OpenApiExtensions
{
    extension(IServiceCollection services)
    {
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
        public OpenApiOptions AddAcceptLanguageHeader()
            => options.AddOperationTransformer<AcceptLanguageHeaderOperationTransformer>();

        public OpenApiOptions AddDefaultProblemDetailsResponse()
        {
#if NET9_0
            options.AddDocumentTransformer<DefaultResponseDocumentTransformer>();
#endif
            options.AddOperationTransformer<DefaultResponseOperationTransformer>();

            return options;
        }

        public OpenApiOptions AddOperationParameters()
            => options.AddOperationTransformer<OpenApiParametersOperationFilter>();

        public OpenApiOptions RemoveServerList()
            => options.AddDocumentTransformer<RemoveServerListDocumentTransformer>();

        public OpenApiOptions WriteNumberAsString()
            => options.AddSchemaTransformer<WriteNumberAsStringSchemaTransformer>();

        public OpenApiOptions DescribeAllParametersInCamelCase()
            => options.AddOperationTransformer<CamelCaseQueryParametersOperationTransformer>();

        public OpenApiOptions AddTimeExamples()
            => options.AddSchemaTransformer<TimeExampleSchemaTransformer>();

#if NET9_0
        public OpenApiOptions EnableEnumSupport()
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
        public OpenApiOptions UseStrictNumericSchemas()
            => options.AddSchemaTransformer<StrictNumericSchemaTransformer>();
#endif
    }
}

#endif