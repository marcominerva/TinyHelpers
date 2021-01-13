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
        public static string? GetDescription(this Enum @enum)
        {
            var descriptions = new List<string>();

            // Get the type
            var type = @enum.GetType();

            foreach (var item in @enum.GetFlags())
            {
                // Get FieldInfo for this type
                var enumDescription = item.ToString();

                var fieldInfo = type.GetRuntimeField(enumDescription);
                enumDescription = GetFieldInfoDescription(fieldInfo, defaultValue: enumDescription);

                descriptions.Add(enumDescription);
            }

            return string.Join(", ", descriptions);
        }

        public static IEnumerable<T> GetFlags<T>(this T @enum) where T : Enum
            => Enum.GetValues(@enum.GetType()).Cast<T>().Where(e => @enum.HasFlag(e));

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
