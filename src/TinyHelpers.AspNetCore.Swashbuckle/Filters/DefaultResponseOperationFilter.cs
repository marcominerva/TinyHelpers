using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHelpers.AspNetCore.Swagger.Filters;

#if NET11_0_OR_GREATER

/// <summary>
/// Adds a default <c>application/problem+json</c> response to an OpenAPI operation.
/// </summary>
/// <remarks>
/// The filter ensures <see cref="ProblemDetails" /> is present in the schema repository so error
/// responses are documented consistently across endpoints.
/// </remarks>
internal class DefaultResponseOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository);

        operation.Responses ??= [];
        operation.Responses.TryAdd("default", new OpenApiResponse
        {
            Description = "Error",
            Content = new Dictionary<string, IOpenApiMediaType>
            {
                [MediaTypeNames.Application.ProblemJson] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchemaReference(nameof(ProblemDetails), context.Document)
                }
            }
        });
    }
}

#else

/// <summary>
/// Adds a default <c>application/problem+json</c> response to an OpenAPI operation.
/// </summary>
/// <remarks>
/// The filter ensures <see cref="ProblemDetails" /> is present in the schema repository so error
/// responses are documented consistently across endpoints.
/// </remarks>
internal class DefaultResponseOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository);

        operation.Responses ??= [];
        operation.Responses.TryAdd("default", new OpenApiResponse
        {
            Description = "Error",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [MediaTypeNames.Application.ProblemJson] = new()
                {
                    Schema = new OpenApiSchemaReference(nameof(ProblemDetails), context.Document)
                }
            }
        });
    }
}

#endif