using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
#if NET9_0_OR_GREATER
using Microsoft.OpenApi;
#endif

namespace TinyHelpers.AspNetCore.Extensions;

/// <summary>
/// Extension methods for <see cref="RouteHandlerBuilder"/>.
/// </summary>
/// <seealso cref="RouteHandlerBuilder"/>
public static class RouteHandlerBuilderExtensions
{
    extension(RouteHandlerBuilder builder)
    {
        /// <summary>
        /// Adds to <see cref="RouteHandlerBuilder"/> the specified list of status codes as <see cref="ProblemDetails"/> responses.
        /// </summary>
        /// <param name="builder">The <see cref="RouteHandlerBuilder"/>.</param>
        /// <param name="statusCodes">The list of status codes to be added as <see cref="ProblemDetails"/> responses.</param>
        /// <returns>The <see cref="RouteHandlerBuilder"/> with the new status codes responses.</returns>
        public RouteHandlerBuilder ProducesDefaultProblem(params int[] statusCodes)
        {
            foreach (var statusCode in statusCodes)
            {
                builder.ProducesProblem(statusCode);
            }

            return builder;
        }

#if NET10_0_OR_GREATER
        public RouteHandlerBuilder WithResponseDescription(int statusCode, string description)
        {
            builder.AddOpenApiOperationTransformer((operation, _, _) =>
            {
                if (operation.Responses?.TryGetValue(statusCode.ToString(), out var response) == true)
                {
                    response.Description = description;
                }

                return Task.CompletedTask;
            });

            return builder;
        }

        public RouteHandlerBuilder WithLocationHeader(string description = "Location of the created resource", int statusCode = StatusCodes.Status201Created)
        {
            builder.AddOpenApiOperationTransformer((operation, _, _) =>
            {
                if (operation.Responses?.TryGetValue(statusCode.ToString(), out var response) == true)
                {
                    if (response is OpenApiResponse openApiResponse && openApiResponse.Headers == null)
                    {
                        openApiResponse.Headers = new Dictionary<string, IOpenApiHeader>();
                    }

                    response.Headers?["Location"] = new OpenApiHeader
                    {
                        Description = description,
                        Required = true,
                        Schema = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Format = "uri"
                        }
                    };
                }

                return Task.CompletedTask;
            });

            return builder;
        }
#endif
    }
}
