using Microsoft.OpenApi.Models;

namespace TinyHelpers.AspNetCore.Swagger.Filters;

public class OpenApiOperationOptions
{
    internal OpenApiOperationOptions()
    {
    }

    public IList<OpenApiParameter> Parameters { get; } = [];
}
