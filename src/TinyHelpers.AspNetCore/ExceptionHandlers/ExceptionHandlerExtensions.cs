using Microsoft.Extensions.DependencyInjection;

namespace TinyHelpers.AspNetCore.ExceptionHandlers;

#if NET8_0_OR_GREATER
public static class ExceptionHandlerExtensions
{
    public static IServiceCollection AddDefaultExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<DefaultExceptionHandler>();
        return services;
    }
}
#endif
