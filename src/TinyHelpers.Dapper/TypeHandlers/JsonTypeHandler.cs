using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public JsonTypeHandler(JsonSerializerOptions? jsonSerializerOptions = null)
    {
        this.jsonSerializerOptions = jsonSerializerOptions ?? JsonOptions.Default;
    }

    public override T Parse(object value)
    {
        var json = value.ToString()!;
        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
    }

    public override void SetValue(IDbDataParameter parameter, T value)
    {
        var json = JsonSerializer.Serialize<object>(value!, jsonSerializerOptions);
        parameter.Value = json;
    }

    public static void Configure(JsonSerializerOptions? jsonSerializerOptions = null, bool useUtcDate = false, bool serializeEnumAsString = false)
    {
        jsonSerializerOptions ??= new(JsonOptions.Default);

        if (useUtcDate)
        {
            jsonSerializerOptions.Converters.Add(new Json.Serialization.UtcDateTimeConverter());
        }

        if (serializeEnumAsString)
        {
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        SqlMapper.AddTypeHandler(new JsonTypeHandler<T>(jsonSerializerOptions));
    }
}
