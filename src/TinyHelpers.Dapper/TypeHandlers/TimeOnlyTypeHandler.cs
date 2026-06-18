using System.Data;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

#if !NETSTANDARD2_0
/// <summary>
/// Provides a Dapper type handler for <see cref="TimeOnly" /> values.
/// </summary>
/// <remarks>
/// The handler keeps time-only values independent from a specific date so database mappings stay
/// semantically correct and avoid implicit date assumptions.
/// </remarks>
public class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
    /// <inheritdoc />
    public override TimeOnly Parse(object value)
        => TimeOnly.FromTimeSpan(TimeSpan.Parse(value.ToString()!));

    /// <inheritdoc />
    public override void SetValue(IDbDataParameter parameter, TimeOnly value)
    {
        parameter.DbType = DbType.Time;
        parameter.Value = value.ToTimeSpan();
    }

    /// <summary>
    /// Registers <see cref="TimeOnlyTypeHandler" /> with Dapper for application-wide use.
    /// </summary>
    /// <remarks>
    /// Register this once during application startup so Dapper can transparently map time-only values in both query
    /// parameters and result sets without coupling them to a calendar date.
    /// </remarks>
    public static void Configure()
        => SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
}
#endif
