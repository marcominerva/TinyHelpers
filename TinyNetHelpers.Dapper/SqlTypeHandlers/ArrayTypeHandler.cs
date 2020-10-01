using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace TinyNetHelpers.Dapper.SqlTypeHandlers
{
    public class ArrayTypeHandler : SqlMapper.TypeHandler<IEnumerable<string>>
    {
        private readonly string separator;

        public ArrayTypeHandler(string separator = ";")
        {
            this.separator = separator;
        }

        public override IEnumerable<string> Parse(object value)
        {
            var content = value.ToString();
            return content.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override void SetValue(IDbDataParameter parameter, IEnumerable<string> value)
        {
            var content = string.Join(separator, value);
            parameter.Value = content;
        }

        public static void Configure(string separator = ";")
            => SqlMapper.AddTypeHandler(new ArrayTypeHandler(separator));
    }
}
