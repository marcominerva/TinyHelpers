using System.Data;
using Dapper;

namespace TinyHelpers.Dapper.TypeHandlers;

public class StringEnumerableTypeHandler : SqlMapper.TypeHandler<IEnumerable<string>>
{
    private readonly string separator;

    public StringEnumerableTypeHandler(string separator = ";")
    {
        this.separator = separator;
    }

    public override IEnumerable<string> Parse(object value)
    {
        var content = value.ToString()!;
        return content.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
    }

    public override void SetValue(IDbDataParameter parameter, IEnumerable<string> value)
    {
        var content = string.Join(separator, value);
        parameter.Value = content;
    }

    public static void Configure(string separator = ";")
        => SqlMapper.AddTypeHandler(new StringEnumerableTypeHandler(separator));
}
