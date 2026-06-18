#if NET9_0

using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

/// <summary>
/// Collects reusable OpenAPI operation metadata that should be applied consistently across multiple endpoints.
/// </summary>
/// <remarks>
/// The library uses this options object to register shared parameter definitions once and copy them into the
/// generated OpenAPI document wherever they are needed. This keeps endpoint setup centralized and avoids drift between
/// route metadata and the generated client contract.
/// </remarks>
public class OpenApiOperationOptions
{
    internal OpenApiOperationOptions()
    {
    }

    /// <summary>
    /// Gets the parameter definitions that should be merged into generated OpenAPI operations.
    /// </summary>
    /// <remarks>
    /// Parameters are intentionally stored here rather than declared inline on each route so callers can reuse metadata
    /// for cross-cutting inputs such as headers or query values while keeping the generated contract consistent.
    /// </remarks>
    public IList<OpenApiParameter> Parameters { get; } = [];
}

#elif NET10_0_OR_GREATER

using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi;

/// <summary>
/// Collects reusable OpenAPI operation metadata that should be applied consistently across multiple endpoints.
/// </summary>
/// <remarks>
/// The library uses this options object to register shared parameter definitions once and copy them into the
/// generated OpenAPI document wherever they are needed. This keeps endpoint setup centralized and avoids drift between
/// route metadata and the generated client contract.
/// </remarks>
public class OpenApiOperationOptions
{
    internal OpenApiOperationOptions()
    {
    }

    /// <summary>
    /// Gets the parameter definitions that should be merged into generated OpenAPI operations.
    /// </summary>
    /// <remarks>
    /// Parameters are intentionally stored here rather than declared inline on each route so callers can reuse metadata
    /// for cross-cutting inputs such as headers or query values while keeping the generated contract consistent.
    /// </remarks>
    public IList<OpenApiParameter> Parameters { get; } = [];
}

#endif