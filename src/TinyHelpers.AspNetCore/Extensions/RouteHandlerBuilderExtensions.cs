using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#if NET9_0_OR_GREATER
using Microsoft.OpenApi;
#endif

namespace TinyHelpers.AspNetCore.Extensions;

/// <summary>
/// Adds minimal OpenAPI-focused helpers to <see cref="RouteHandlerBuilder" /> so endpoint metadata stays close to the route mapping code.
/// </summary>
/// <seealso cref="RouteHandlerBuilder"/>
public static class RouteHandlerBuilderExtensions
{
    /// <summary>
    /// Declares a set of problem responses on the endpoint.
    /// </summary>
    /// <param name="builder">The route configuration being extended.</param>
    /// <param name="statusCodes">The HTTP status codes that should be described as <see cref="ProblemDetails" /> responses.</param>
    /// <returns>The same <see cref="RouteHandlerBuilder" /> so calls can be chained.</returns>
    public static RouteHandlerBuilder ProducesDefaultProblem(this RouteHandlerBuilder builder, params int[] statusCodes)
    {
        foreach (var statusCode in statusCodes)
        {
            builder.ProducesProblem(statusCode);
        }

        return builder;
    }

    extension(RouteHandlerBuilder builder)
    {
#if NET10_0_OR_GREATER
        /// <summary>
        /// Updates the OpenAPI description for a specific response code when the default generated text is too generic.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to annotate.</param>
        /// <param name="description">The text to show in the generated OpenAPI document.</param>
        /// <returns>The same <see cref="RouteHandlerBuilder" /> so additional configuration can continue fluently.</returns>
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

        /// <summary>
        /// Adds a required <c>Location</c> header to creation responses so consumers know where to find the created resource.
        /// </summary>
        /// <param name="description">The OpenAPI description for the <c>Location</c> header.</param>
        /// <param name="statusCode">The status code whose response should advertise the header.</param>
        /// <returns>The same <see cref="RouteHandlerBuilder" /> so additional configuration can continue fluently.</returns>
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
