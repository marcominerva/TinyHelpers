using System.Data;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

#if !NETSTANDARD2_0
/// <summary>
/// Provides a Dapper type handler for <see cref="DateOnly" /> values.
/// </summary>
/// <remarks>
/// The handler maps dates to the database <see cref="DbType.Date" /> representation so date-only values
/// can be round-tripped without adding an artificial time component.
/// </remarks>
public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    /// <inheritdoc />
    public override DateOnly Parse(object value)
        => DateOnly.FromDateTime(Convert.ToDateTime(value));

    /// <inheritdoc />
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    /// <summary>
    /// Registers <see cref="DateOnlyTypeHandler" /> with Dapper for application-wide use.
    /// </summary>
    /// <remarks>
    /// Register this once during application startup when you want Dapper to automatically convert date-only values in
    /// query results and parameters without repeating manual <see cref="DateTime" /> conversions.
    /// </remarks>
    public static void Configure()
        => SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
}
#endif