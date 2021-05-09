using Dapper;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Dapper.SqlTypeHandlers
{
    public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        private static readonly JsonSerializerOptions defaultJsonSerializerOptions;

        static JsonTypeHandler()
        {
            defaultJsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            defaultJsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        private readonly JsonSerializerOptions jsonSerializerOptions;

        public JsonTypeHandler(JsonSerializerOptions? jsonSerializerOptions = null)
        {
            this.jsonSerializerOptions = jsonSerializerOptions ?? defaultJsonSerializerOptions;
        }

        public override T Parse(object value)
        {
            var json = value.ToString();
            return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
        }

        public override void SetValue(IDbDataParameter parameter, T value)
        {
            var json = JsonSerializer.Serialize(value, jsonSerializerOptions);
            parameter.Value = json;
        }

        public static void Configure(JsonSerializerOptions? jsonSerializerOptions = null)
            => SqlMapper.AddTypeHandler(new JsonTypeHandler<T>(jsonSerializerOptions));
    }
}
