using Microsoft.OpenApi;

namespace TinyHelpers.AspNetCore.Swagger.Filters;

public class OpenApiOperationOptions
{
    internal OpenApiOperationOptions()
    {
    }

    public IList<OpenApiParameter> Parameters { get; } = [];
}
