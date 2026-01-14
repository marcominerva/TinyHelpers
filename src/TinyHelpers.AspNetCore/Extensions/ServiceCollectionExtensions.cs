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

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public T? ConfigureAndGet<T>(IConfiguration configuration, string sectionName) where T : class
        {
            var section = configuration.GetSection(sectionName);
            var settings = section.Get<T>();
            services.Configure<T>(section);

            return settings;
        }

        public IServiceCollection AddRequestLocalization(params string[] cultures)
            => services.AddRequestLocalization(cultures, null);

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

        public IServiceCollection Replace<TService, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TService : class
            where TImplementation : class, TService
        {
            services.Replace(new ServiceDescriptor(serviceType: typeof(TService), implementationType: typeof(TImplementation), lifetime));

            return services;
        }

        public IServiceCollection AddDefaultProblemDetails()
        {
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context => context.UseDefaults();
            });

            return services;
        }

        public IServiceCollection AddDefaultExceptionHandler()
        {
            // Ensures that the ProblemDetails service is registered.
            services.AddProblemDetails();

            services.AddExceptionHandler<DefaultExceptionHandler>();
            return services;
        }
    }

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