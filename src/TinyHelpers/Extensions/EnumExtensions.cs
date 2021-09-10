using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace TinyHelpers.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            var descriptions = new List<string>();
            var type = @enum.GetType();

            foreach (var item in @enum.GetFlags())
            {
                var enumDescription = item.ToString();

                var fieldInfo = type.GetRuntimeField(enumDescription);
                enumDescription = GetFieldInfoDescription(fieldInfo, defaultValue: enumDescription);

                descriptions.Add(enumDescription);
            }

            if (descriptions.Any())
            {
                return string.Join(", ", descriptions);
            }

            return @enum.ToString();
        }

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

            return Enumerable.Empty<T>();
        }

        public static Dictionary<int, string> GetDescriptions(this Type enumType)
        {
            var descriptions = new Dictionary<int, string>();
            var fields = enumType.GetRuntimeFields().Where(f => f.IsStatic).ToList();

            foreach (var fieldInfo in fields)
            {
                var value = (int)fieldInfo.GetValue(null);
                var enumDescription = GetFieldInfoDescription(fieldInfo, defaultValue: Enum.GetName(enumType, value));

                descriptions.Add(value, enumDescription);
            }

            return descriptions;
        }

        private static string GetFieldInfoDescription(FieldInfo fieldInfo, string defaultValue)
        {
            var enumDescription = defaultValue;

            var displayAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (displayAttributes.Any())
            {
                // Return the first if there was a match.
                var displayAttribute = displayAttributes.First();
                if (displayAttribute.ResourceType == null)
                {
                    enumDescription = displayAttribute.Name;
                }
                else
                {
                    var resourceManager = new ResourceManager(displayAttribute.ResourceType);
                    enumDescription = resourceManager.GetString(displayAttribute.Name);
                }
            }

            return enumDescription;
        }
    }
}
