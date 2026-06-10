using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.EntityFrameworkCore;

internal class JsonOptions
{
    /// <summary>
    /// Gets the shared JSON serializer settings used by the Entity Framework Core converters and comparers.
    /// </summary>
    /// <remarks>
    /// The options are centralized so all JSON-based mappings use the same defaults, which keeps serialization
    /// behavior predictable and avoids subtle mismatch bugs between converters and comparers.
    /// </remarks>
    public static JsonSerializerOptions Default { get; }

    static JsonOptions()
    {
        Default = new(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }
}
