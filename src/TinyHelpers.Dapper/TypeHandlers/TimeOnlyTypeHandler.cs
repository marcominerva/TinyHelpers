using System.Data;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

#if NET6_0_OR_GREATER
public class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
    public override TimeOnly Parse(object value)
        => TimeOnly.FromTimeSpan(TimeSpan.Parse(value.ToString()!));

    public override void SetValue(IDbDataParameter parameter, TimeOnly value)
    {
        parameter.DbType = DbType.Time;
        parameter.Value = value.ToTimeSpan();
    }

    public static void Configure()
        => SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
}
#endif
