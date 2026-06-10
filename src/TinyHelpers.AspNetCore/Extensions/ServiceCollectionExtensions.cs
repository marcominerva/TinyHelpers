using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TinyHelpers.AspNetCore.ExceptionHandlers;

namespace TinyHelpers.AspNetCore.Extensions;

/// <summary>
/// Registers common application services and configuration helpers that the library reuses across samples and packages.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Binds a configuration section and registers the same instance with the options system so callers only describe the settings once.
    /// </summary>
    /// <typeparam name="T">The options type to bind.</typeparam>
    /// <param name="services">The service collection being configured.</param>
    /// <param name="configuration">The configuration root that contains the section.</param>
    /// <param name="sectionName">The section path to bind.</param>
    /// <returns>The bound settings instance, or <see langword="null" /> if the section could not be materialized.</returns>
    public static T? ConfigureAndGet<T>(this IServiceCollection services, IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName);
        var settings = section.Get<T>();
        services.Configure<T>(section);

        return settings;
    }

    /// <summary>
    /// Registers request localization with a culture list.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <param name="cultures">The supported culture names.</param>
    /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
    /// <remarks>The first culture becomes the default.</remarks>
    public static IServiceCollection AddRequestLocalization(this IServiceCollection services, params string[] cultures)
        => services.AddRequestLocalization(cultures, null);

    /// <summary>
    /// Registers request localization and allows the caller to adjust the culture-provider chain.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <param name="cultures">The supported culture names.</param>
    /// <param name="providersConfiguration">A callback that can reorder or replace the request culture providers.</param>
    /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
    /// <remarks>The first culture becomes the default.</remarks>
    public static IServiceCollection AddRequestLocalization(this IServiceCollection services, IEnumerable<string> cultures, Action<IList<IRequestCultureProvider>>? providersConfiguration)
    {
        var supportedCultures = cultures.Select(c => new CultureInfo(c)).ToList();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
            providersConfiguration?.Invoke(options.RequestCultureProviders);
        });

        return services;
    }

    /// <summary>
    /// Replaces one registered service with another implementation while preserving the desired lifetime.
    /// </summary>
    /// <typeparam name="TService">The service contract to replace.</typeparam>
    /// <typeparam name="TImplementation">The implementation to register.</typeparam>
    /// <param name="services">The service collection being modified.</param>
    /// <param name="lifetime">The lifetime that should be used for the replacement registration.</param>
    /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
    public static IServiceCollection Replace<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class
        where TImplementation : class, TService
    {
        services.Replace(new ServiceDescriptor(serviceType: typeof(TService), implementationType: typeof(TImplementation), lifetime));

        return services;
    }

    /// <summary>
    /// Registers a predictable <see cref="ProblemDetails" /> pipeline so the application emits consistent error payloads.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
    public static IServiceCollection AddDefaultProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context => context.UseDefaults();
        });

        return services;
    }

    /// <summary>
    /// Registers the library default exception handler so uncaught failures are shaped into problem details before they leave the pipeline.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
    public static IServiceCollection AddDefaultExceptionHandler(this IServiceCollection services)
    {
        // Ensures that the ProblemDetails service is registered.
        services.AddProblemDetails();

        services.AddExceptionHandler<DefaultExceptionHandler>();
        return services;
    }

    /// <summary>
    /// Applies the library default values to a <see cref="ProblemDetailsContext" /> so callers get consistent and useful diagnostics.
    /// </summary>
    /// <param name="context">The problem-details context to normalize.</param>
    public static void UseDefaults(this ProblemDetailsContext context)
    {
        var statusCode = context.ProblemDetails.Status.GetValueOrDefault(StatusCodes.Status500InternalServerError);

        context.ProblemDetails.Type ??= $"https://httpstatuses.io/{statusCode}";
        context.ProblemDetails.Title ??= ReasonPhrases.GetReasonPhrase(statusCode);
        context.ProblemDetails.Instance ??= context.HttpContext.Request.Path;
        context.ProblemDetails.Detail ??= context.Exception?.Message;

        context.ProblemDetails.Extensions.TryAdd("traceId", Activity.Current?.Id ?? context.HttpContext.TraceIdentifier);
    }
}