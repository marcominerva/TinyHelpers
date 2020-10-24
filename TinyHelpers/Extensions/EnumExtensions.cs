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
                if (fieldInfo == null)
                {
                    return null;
                }

                // Get the Display attributes
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

                descriptions.Add(enumDescription);
            }

            return string.Join(", ", descriptions);
        }

        public static IEnumerable<T> GetFlags<T>(this T @enum) where T : Enum
            => Enum.GetValues(@enum.GetType()).Cast<T>().Where(e => @enum.HasFlag(e));
    }
}
