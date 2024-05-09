using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace TinyHelpers.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="Enum"/> type.
/// </summary>
/// <seealso cref="Enum"/>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the description of the provided enum value. The description is retrieved from the <see cref="DisplayAttribute"/>, if present, and supports localization.
    /// </summary>
    /// <param name="enum">The enum for which to get the description.</param>
    /// <returns>The description of the enum.</returns>
    /// <seealso cref="DisplayAttribute"/>
    /// <seealso cref="Enum"/>
    public static string GetDescription(this Enum @enum)
    {
        var descriptions = new List<string?>();
        var type = @enum.GetType();

        foreach (var item in @enum.GetFlags())
        {
            var enumDescription = item.ToString();

            var fieldInfo = type.GetRuntimeField(enumDescription)!;
            enumDescription = GetFieldInfoDescription(fieldInfo, defaultValue: enumDescription);

            descriptions.Add(enumDescription);
        }

        if (descriptions.Any())
        {
            return string.Join(", ", descriptions);
        }

        return @enum.ToString();
    }

    /// <summary>
    /// Provides an <see cref="IEnumerable{T}"/> that represents each flag enum values.
    /// </summary>
    /// <param name="enum">The enum to get values.</param>
    /// <returns>The <see cref="IEnumerable{T}"/> that represents each flag enum values.</returns>
    /// <seealso cref="Enum"/>
    public static IEnumerable<T> GetFlags<T>(this T @enum) where T : Enum
    {
        var values = Enum.GetValues(@enum.GetType()).Cast<T>();
        var bits = Convert.ToInt64(@enum);
        var results = new List<T>();

        for (var i = values.Count() - 1; i >= 0; i--)
        {
            var mask = Convert.ToInt64(values.ElementAt(i));
            if (i == 0 && mask == 0L)
            {
                break;
            }

            if ((bits & mask) == mask)
            {
                results.Add(values.ElementAt(i));
                bits -= mask;
            }
        }

        if (bits != 0L)
        {
            return Enumerable.Empty<T>();
        }

        if (Convert.ToInt64(@enum) != 0L)
        {
            return results.Reverse<T>();
        }

        if (bits == Convert.ToInt64(@enum) && values.Any() && Convert.ToInt64(values.ElementAt(0)) == 0L)
        {
            return values.Take(1);
        }

        return [];
    }

    /// <summary>
    /// Gets the descriptions of all values of the provided enum type. Each description is retrieved from the <see cref="DisplayAttribute"/>, if present, and supports localization.
    /// </summary>
    /// <typeparam name="T">The enum type for which to get the descriptions.</typeparam>
    /// <returns>A dictionary containing the descriptions of each enum value.</returns>
    /// <seealso cref="DisplayAttribute"/>
    /// <exception cref="ArgumentException">The provided type is not an enum.</exception>
    /// <seealso cref="GetDescription(Enum)"/>
    /// <seealso cref="GetDescriptions(Type)"/>
    /// <seealso cref="DisplayAttribute"/>
    /// <seealso cref="Enum"/>
    public static Dictionary<int, string?> GetDescriptions<T>() where T : Enum
        => GetDescriptions(typeof(T));

    /// <summary>
    /// Gets the descriptions of all values of the provided enum type. Each description is retrieved from the <see cref="DisplayAttribute"/>, if present, and supports localization.
    /// </summary>
    /// <param name="enumType">The type on the enum for which to get the descriptions.</param>
    /// <returns>A dictionary containing the descriptions of each enum value.</returns>
    /// <seealso cref="DisplayAttribute"/>
    /// <exception cref="ArgumentException">The provided type is not an enum.</exception>
    /// <seealso cref="GetDescription(Enum)"/>
    /// <seealso cref="GetDescriptions{T}()"/>
    /// <seealso cref="DisplayAttribute"/>
    /// <seealso cref="Enum"/>
    public static Dictionary<int, string?> GetDescriptions(this Type enumType)
    {
        var descriptions = new Dictionary<int, string?>();
        var fields = enumType.GetRuntimeFields().Where(f => f.IsStatic).ToList();

        foreach (var fieldInfo in fields)
        {
            var value = (int)fieldInfo.GetValue(null)!;
            var enumDescription = GetFieldInfoDescription(fieldInfo, defaultValue: Enum.GetName(enumType, value));

            descriptions.Add(value, enumDescription);
        }

        return descriptions;
    }

    private static string? GetFieldInfoDescription(FieldInfo fieldInfo, string? defaultValue)
    {
        string? enumDescription;

        var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute?.ResourceType is null)
        {
            enumDescription = displayAttribute?.Name;
        }
        else
        {
            var resourceManager = new ResourceManager(displayAttribute.ResourceType);
            enumDescription = resourceManager.GetString(displayAttribute.Name!) ?? displayAttribute.Name;
        }

        return enumDescription ?? defaultValue;
    }
}
