using System.ComponentModel;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TinyHelpers.AspNetCore.TypeConverters;

namespace TinyHelpers.AspNetCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static T? ConfigureAndGet<T>(this IServiceCollection services, IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName);
        var settings = section.Get<T>();
        services.Configure<T>(section);

        return settings;
    }

#if NET6_0
    public static IServiceCollection AddDateOnlyTimeOnly(this IServiceCollection services)
    {
        TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
        TypeDescriptor.AddAttributes(typeof(TimeOnly), new TypeConverterAttribute(typeof(TimeOnlyTypeConverter)));

        return services;
    }
#endif

    public static IServiceCollection AddRequestLocalization(this IServiceCollection services, params string[] cultures)
        => services.AddRequestLocalization(cultures, null);

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

    public static IServiceCollection Replace<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class
        where TImplementation : class, TService
    {
        services.Replace(new ServiceDescriptor(serviceType: typeof(TService), implementationType: typeof(TImplementation), lifetime));

        return services;
    }
}