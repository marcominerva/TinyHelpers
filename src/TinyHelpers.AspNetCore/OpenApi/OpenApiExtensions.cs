#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;

namespace TinyHelpers.AspNetCore.OpenApi;

public static class OpenApiExtensions
{
    public static void AddAcceptLanguageHeader(this OpenApiOptions options)
        => options.AddOperationTransformer<AcceptLanguageHeaderOperationTransformer>();

}

#endif