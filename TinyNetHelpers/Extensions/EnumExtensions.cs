using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace TinyNetHelpers.Extensions
{
    public static class EnumExtensions
    {
        public static string? GetDescription(this Enum @enum)
        {
            // Get the type
            var type = @enum.GetType();

            // Get FieldInfo for this type
            var fieldInfo = type.GetRuntimeField(@enum.ToString());
            if (fieldInfo == null)
            {
                return null;
            }

            // Get the Display attributes
            var displayAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            var description = @enum.ToString();
            if (displayAttributes.Any())
            {
                // Return the first if there was a match.
                var displayAttribute = displayAttributes.First();
                if (displayAttribute.ResourceType == null)
                {
                    description = displayAttribute.Name;
                }
                else
                {
                    var resourceManager = new ResourceManager(displayAttribute.ResourceType);
                    description = resourceManager.GetString(displayAttribute.Name);
                }
            }

            return description;
        }
    }
}
