using System.Data;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

public class StringEnumerableTypeHandler(string separator = ";") : SqlMapper.TypeHandler<IEnumerable<string>>
{
    public override IEnumerable<string> Parse(object value)
    {
        var content = value.ToString()!;
        return content.Split([separator], StringSplitOptions.RemoveEmptyEntries);
    }

    public override void SetValue(IDbDataParameter parameter, IEnumerable<string>? value)
    {
        var content = string.Join(separator, value!);
        parameter.Value = content;
    }

    public static void Configure(string separator = ";")
        => SqlMapper.AddTypeHandler(new StringEnumerableTypeHandler(separator));
}
