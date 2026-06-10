using System.Data;
using System.Globalization;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

/// <summary>
/// Provides a Dapper type handler for <see cref="TimeSpan" /> values.
/// </summary>
/// <remarks>
/// This handler stores durations as <see cref="long" /> tick values so database reads and writes remain
/// culture-independent and stable across the supported target frameworks.
/// </remarks>
public class TimeSpanTypeHandler : SqlMapper.TypeHandler<TimeSpan>
{
    /// <inheritdoc />
    public override TimeSpan Parse(object value)
    {
        return value switch
        {
            null => TimeSpan.Zero,
            string stringValue => TimeSpan.Parse(stringValue, CultureInfo.InvariantCulture),
            long longValue => TimeSpan.FromTicks(longValue),
            int intValue => TimeSpan.FromSeconds(intValue),
            double doubleValue => TimeSpan.FromSeconds(doubleValue),
            _ => throw new ArgumentException($"Unable to convert {value.GetType()} to {nameof(TimeSpan)}"),
        };
    }

    /// <inheritdoc />
    public override void SetValue(IDbDataParameter parameter, TimeSpan value)
    {
        parameter.Value = value.Ticks;
        parameter.DbType = DbType.Int64;
    }

    /// <summary>
    /// Registers <see cref="TimeSpanTypeHandler" /> with Dapper for application-wide use.
    /// </summary>
    /// <remarks>
    /// Call this once during application startup so Dapper can transparently map <see cref="TimeSpan" /> values
    /// to and from the underlying database representation.
    /// </remarks>
    public static void Configure()
        => SqlMapper.AddTypeHandler(new TimeSpanTypeHandler());
}
