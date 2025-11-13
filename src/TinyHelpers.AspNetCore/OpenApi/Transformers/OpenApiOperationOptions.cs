#if NET9_0

using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

public class OpenApiOperationOptions
{
    internal OpenApiOperationOptions()
    {
    }

    public IList<OpenApiParameter> Parameters { get; } = [];
}

#elif NET10_0_OR_GREATER

using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi;

public class OpenApiOperationOptions
{
    internal OpenApiOperationOptions()
    {
    }

    public IList<OpenApiParameter> Parameters { get; } = [];
}

#endif