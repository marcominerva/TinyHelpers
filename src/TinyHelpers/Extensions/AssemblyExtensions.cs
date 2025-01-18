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
    /// <typeparam name="TValue">The type of the value to extract from the attribute.</typeparam>
    /// <param name="assembly">The assembly from which to retrieve the attribute.</param>
    /// <param name="value">
    /// A delegate that specifies how to extract a value of type <typeparamref name="TValue"/> from the attribute.
    /// </param>
    /// <returns>
    /// A value of type <typeparamref name="TValue"/> extracted from the attribute using the delegate, or <c>default</c> if the attribute is not found.
    /// </returns>
    public static TValue? GetAttribute<T, TValue>(this Assembly assembly, Func<T, TValue> value)
        where T : Attribute
    {
        if (assembly?
            .GetCustomAttributes(typeof(T), false)
            .OfType<T>()
            .FirstOrDefault() is T attribute)
        {
            return value(attribute);
        }

        return default;
    }
}
