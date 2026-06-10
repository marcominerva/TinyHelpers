using System.Data;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

/// <summary>
/// Provides a Dapper type handler for string sequences serialized as a delimited string.
/// </summary>
/// <remarks>
/// This handler is intended for database columns that store multiple string values in a single field while
/// keeping the consuming code focused on <see cref="IEnumerable{T}" /> instead of manual split/join logic.
/// </remarks>
public class StringEnumerableTypeHandler(string separator = ";") : SqlMapper.TypeHandler<IEnumerable<string>>
{
    /// <inheritdoc />
    public override IEnumerable<string> Parse(object value)
    {
        var content = value?.ToString() ?? string.Empty;
        return content.Split([separator], StringSplitOptions.RemoveEmptyEntries);
    }

    /// <inheritdoc />
    public override void SetValue(IDbDataParameter parameter, IEnumerable<string>? value)
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
    /// Registers <see cref="StringEnumerableTypeHandler" /> with Dapper using the specified separator.
    /// </summary>
    /// <remarks>
    /// Keep the separator aligned with the storage format so Dapper can reconstruct the sequence correctly.
    /// </remarks>
    public static void Configure(string separator = ";")
        => SqlMapper.AddTypeHandler(new StringEnumerableTypeHandler(separator));
}
