using System.Reflection;

namespace TinyHelpers.Extensions;

/// <summary>
/// Provides extension methods for working with assembly attributes.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Retrieves a custom attribute of the specified type from an assembly and extracts a value using the provided delegate.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to retrieve.</typeparam>
    /// <param name="assembly">The assembly from which to retrieve the attribute.</param>
    /// <param name="value">
    /// A delegate that specifies how to extract a string value from the attribute.
    /// </param>
    /// <returns>
    /// A string extracted from the attribute using the delegate, or <c>null</c> if the attribute is not found.
    /// </returns>
    public static string? GetAttribute<T>(this Assembly assembly, Func<T, string> value) where T : Attribute
    {
        var attribute = assembly?.GetCustomAttribute<T>();
        return attribute != null ? value(attribute) : null;
    }
}
