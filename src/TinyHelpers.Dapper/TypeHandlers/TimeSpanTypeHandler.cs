using System.Data;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

#if NET6_0_OR_GREATER
public class TimeSpanTypeHandler : SqlMapper.TypeHandler<TimeSpan>
{
    public override TimeSpan Parse(object value)
    {
        return value switch
        {
            null => TimeSpan.Zero,
            string stringValue => TimeSpan.Parse(stringValue),
            long longValue => TimeSpan.FromTicks(longValue),
            int intValue => TimeSpan.FromSeconds(intValue),
            double doubleValue => TimeSpan.FromSeconds(doubleValue),
            _ => throw new ArgumentException($"Unable to convert {value.GetType()} to {nameof(TimeSpan)}"),
        };
    }

    public override void SetValue(IDbDataParameter parameter, TimeSpan value)
    {
        parameter.Value = value.Ticks;
        parameter.DbType = DbType.Int64;
    }

    public static void Configure()
        => SqlMapper.AddTypeHandler(new TimeSpanTypeHandler());
}
#endif
