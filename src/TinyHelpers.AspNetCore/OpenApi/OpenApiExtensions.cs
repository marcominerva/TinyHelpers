#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
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
}

#endif