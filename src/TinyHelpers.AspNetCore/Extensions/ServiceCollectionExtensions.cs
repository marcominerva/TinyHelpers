using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TinyHelpers.AspNetCore.ExceptionHandlers;

namespace TinyHelpers.AspNetCore.Extensions;

/// <summary>
/// Registers common ASP.NET Core services and configuration helpers that reduce repeated setup in applications using the library.
/// </summary>
/// <remarks>
/// These helpers intentionally keep the built-in dependency injection and options patterns visible while providing
/// one-line registration for common localization, replacement, and problem-details conventions.
/// </remarks>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Binds a configuration section and registers the same instance with the options system so callers only describe the settings once.
        /// </summary>
        /// <typeparam name="T">The options type to bind.</typeparam>
        /// <param name="configuration">The configuration root that contains the section.</param>
        /// <param name="sectionName">The section path to bind.</param>
        /// <returns>The bound settings instance, or <see langword="null" /> if the section could not be materialized.</returns>
        public T? ConfigureAndGet<T>(IConfiguration configuration, string sectionName) where T : class
        {
            var section = configuration.GetSection(sectionName);
            var settings = section.Get<T>();
            services.Configure<T>(section);

            return settings;
        }

        /// <summary>
        /// Registers request localization with a culture list when the default provider order is sufficient.
        /// </summary>
        /// <param name="cultures">The supported culture names.</param>
        /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
        /// <remarks>
        /// The first culture becomes the default so applications can define their fallback culture in the same order
        /// they declare supported cultures.
        /// </remarks>
        public IServiceCollection AddRequestLocalization(params string[] cultures)
            => services.AddRequestLocalization(cultures, null);

        /// <summary>
        /// Registers request localization and allows the caller to adjust the culture-provider chain used during negotiation.
        /// </summary>
        /// <param name="cultures">The supported culture names.</param>
        /// <param name="providersConfiguration">A callback that can reorder or replace the request culture providers.</param>
        /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
        /// <remarks>
        /// The first culture becomes the default. The provider callback exists for applications that need a specific
        /// precedence, such as preferring route values over cookies or the <c>Accept-Language</c> header.
        /// </remarks>
        public IServiceCollection AddRequestLocalization(IEnumerable<string> cultures, Action<IList<IRequestCultureProvider>>? providersConfiguration)
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
        /// Replaces one registered service with another implementation when an application needs to override a library or framework default.
        /// </summary>
        /// <typeparam name="TService">The service contract to replace.</typeparam>
        /// <typeparam name="TImplementation">The implementation to register.</typeparam>
        /// <param name="lifetime">The lifetime that should be used for the replacement registration.</param>
        /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
        /// <remarks>
        /// This wraps the standard replacement pattern so callers can state the intended service contract and
        /// implementation without manually creating a <see cref="ServiceDescriptor" />.
        /// </remarks>
        public IServiceCollection Replace<TService, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TService : class
            where TImplementation : class, TService
        {
            services.Replace(new ServiceDescriptor(serviceType: typeof(TService), implementationType: typeof(TImplementation), lifetime));

            return services;
        }

        /// <summary>
        /// Registers a predictable <see cref="ProblemDetails" /> pipeline so the application emits consistent error payloads.
        /// </summary>
        /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
        /// <remarks>
        /// The customization fills common problem-details fields and a trace identifier, which gives clients and logs a
        /// stable shape to correlate errors without requiring each application to repeat the same setup.
        /// </remarks>
        public IServiceCollection AddDefaultProblemDetails()
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
        /// <returns>The same <see cref="IServiceCollection" /> so additional registrations can continue fluently.</returns>
        /// <remarks>
        /// Registering the handler with the problem-details service keeps unexpected failures consistent with explicit
        /// validation and endpoint errors, which simplifies client error handling.
        /// </remarks>
        public IServiceCollection AddDefaultExceptionHandler()
        {
            // Ensures that the ProblemDetails service is registered.
            services.AddProblemDetails();

            services.AddExceptionHandler<DefaultExceptionHandler>();
            return services;
        }
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