using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.Swagger.Filters;

/// <summary>
/// Describes additional OpenAPI parameters that should be attached to every operation.
/// </summary>
/// <remarks>
/// Instances are created through dependency injection and consumed by
/// <see cref="OpenApiParametersOperationFilter" /> to avoid duplicating parameter definitions in
/// multiple Swagger configuration points.
/// </remarks>
public class OpenApiOperationOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenApiOperationOptions" /> class.
    /// </summary>
    internal OpenApiOperationOptions()
    {
    }

    /// <summary>
    /// Gets the parameters that should be appended to generated OpenAPI operations.
    /// </summary>
    public IList<OpenApiParameter> Parameters { get; } = [];
}
