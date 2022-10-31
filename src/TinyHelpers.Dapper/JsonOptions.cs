using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

namespace TinyHelpers.Dapper;

internal class JsonOptions
{
    public static JsonSerializerOptions Default { get; }

    static JsonOptions()
    {
        Default = new(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

#if NET6_0
        Default.Converters.Add(new DateOnlyConverter());
        Default.Converters.Add(new TimeOnlyConverter());
#endif
    }
}
