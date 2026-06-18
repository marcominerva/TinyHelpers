using System.Data;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

/// <summary>
/// Provides a Dapper type handler for string arrays serialized as a delimited string.
/// </summary>
/// <remarks>
/// This handler is useful when a database column stores multiple string values in a single field and the
/// application needs to map them to <see cref="Array" /> values without custom parsing logic at each call site.
/// </remarks>
public class StringArrayTypeHandler(string separator = ";") : SqlMapper.TypeHandler<string[]>
{
    /// <inheritdoc />
    public override string[] Parse(object value)
    {
        var content = value?.ToString() ?? string.Empty;
        return content.Split([separator], StringSplitOptions.RemoveEmptyEntries);
    }

    /// <inheritdoc />
    public override void SetValue(IDbDataParameter parameter, string[]? value)
    {
        if (value is null)
        {
            parameter.Value = DBNull.Value;
            return;
        }

        var content = string.Join(separator, value);
        parameter.Value = content;
    }

    /// <summary>
    /// Registers <see cref="StringArrayTypeHandler" /> with Dapper using the specified separator.
    /// </summary>
    /// <param name="separator">The delimiter used to split database values and join parameter values.</param>
    /// <remarks>
    /// Register this once during application startup. Keep the separator consistent with the database format so arrays
    /// can be round-tripped reliably across all Dapper queries.
    /// </remarks>
    public static void Configure(string separator = ";")
        => SqlMapper.AddTypeHandler(new StringArrayTypeHandler(separator));
}
