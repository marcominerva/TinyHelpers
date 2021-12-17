using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

namespace TinyHelpers.EntityFrameworkCore;

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

        Default.Converters.Add(new DateOnlyConverter());
        Default.Converters.Add(new TimeOnlyConverter());
        Default.Converters.Add(new StringEnumMemberConverter());
        Default.Converters.Add(new UtcDateTimeConverter());
    }
}
