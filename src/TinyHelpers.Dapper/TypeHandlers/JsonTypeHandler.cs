using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dapper;
using TinyHelpers.Json.Serialization;

namespace TinyHelpers.Dapper.TypeHandlers;

/// <summary>
/// Provides a Dapper type handler for values stored as JSON.
/// </summary>
/// <typeparam name="T">The CLR type represented by the JSON payload.</typeparam>
/// <remarks>
/// Centralizing JSON conversion here keeps database mappings consistent and avoids repeating serializer
/// configuration across repositories and services.
/// </remarks>
public class JsonTypeHandler<T>(JsonSerializerOptions? jsonSerializerOptions = null) : SqlMapper.TypeHandler<T>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = jsonSerializerOptions ?? JsonOptions.Default;

    /// <inheritdoc />
    public override T Parse(object value)
    {
        var json = value.ToString()!;
        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions)!;
    }

    /// <inheritdoc />
    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        var json = JsonSerializer.Serialize<object>(value!, jsonSerializerOptions);
        parameter.Value = json;
    }

    /// <summary>
    /// Registers <see cref="JsonTypeHandler{T}" /> with Dapper using the supplied JSON configuration.
    /// </summary>
    /// <param name="jsonSerializerOptions">Optional serializer settings used to control the JSON contract.</param>
    /// <param name="useUtcDate">Indicates whether dates should be normalized to UTC before serialization.</param>
    /// <param name="serializeEnumAsString">Indicates whether enums should be written as strings instead of numbers.</param>
    /// <remarks>
    /// Register the handler during application startup so JSON values are converted consistently in all queries.
    /// </remarks>
    public static void Configure(JsonSerializerOptions? jsonSerializerOptions = null, bool useUtcDate = false, bool serializeEnumAsString = false)
    {
        jsonSerializerOptions ??= new(JsonOptions.Default);

        if (useUtcDate)
        {
            jsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
        }

        if (serializeEnumAsString)
        {
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        SqlMapper.AddTypeHandler(new JsonTypeHandler<T>(jsonSerializerOptions));
    }
}
