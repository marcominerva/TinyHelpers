using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Dapper;

internal class JsonOptions
{
    /// <summary>
    /// Gets the shared <see cref="JsonSerializerOptions" /> instance used by the Dapper JSON handlers.
    /// </summary>
    /// <remarks>
    /// The defaults are centralized so JSON-based type handlers can deserialize and serialize values
    /// consistently without repeating configuration across the library.
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
