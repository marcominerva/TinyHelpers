using System.Data;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

#if NET6_0_OR_GREATER
public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
        => DateOnly.FromDateTime(Convert.ToDateTime(value));

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    public static void Configure()
        => SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
}
#endif