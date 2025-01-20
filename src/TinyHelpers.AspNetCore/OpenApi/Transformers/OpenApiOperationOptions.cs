#if NET9_0_OR_GREATER

using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.OpenApi;

public class OpenApiOperationOptions
{
    internal OpenApiOperationOptions()
    {
    }

    public IList<OpenApiParameter> Parameters { get; } = [];
}

#endif