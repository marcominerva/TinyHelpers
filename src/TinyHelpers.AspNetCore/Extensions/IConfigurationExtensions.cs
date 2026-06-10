using Microsoft.Extensions.Configuration;

namespace TinyHelpers.AspNetCore.Extensions;

/// <summary>
/// Provides configuration helpers that bind a section and return the materialized options object in one call.
/// </summary>
public static class IConfigurationExtensions
{
    /// <summary>
    /// Reads a strongly typed settings object from the specified configuration section without forcing callers to repeat the bind logic.
    /// </summary>
    /// <typeparam name="T">The settings type to bind.</typeparam>
    /// <param name="configuration">The configuration root to read from.</param>
    /// <param name="sectionName">The section path that contains the settings.</param>
    /// <returns>The bound settings instance, or <see langword="null" /> when the section cannot be materialized.</returns>
    public static T? GetSection<T>(this IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName);
        var settings = section.Get<T>();

        return settings;
    }
}