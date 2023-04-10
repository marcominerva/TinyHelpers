using Microsoft.Extensions.Configuration;

namespace TinyHelpers.AspNetCore.Extensions;

public static class IConfigurationExtensions
{
    public static T? GetSection<T>(this IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName);
        var settings = section.Get<T>();

        return settings;
    }
}