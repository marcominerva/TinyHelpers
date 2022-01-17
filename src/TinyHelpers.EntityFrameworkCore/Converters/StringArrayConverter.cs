using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

public class StringArrayConverter : ValueConverter<IEnumerable<string>, string>
{
    public StringArrayConverter(string separator = ";") : base(
            list => string.Join(separator, list),
            value => value == null ? Array.Empty<string>() : value.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    {
    }
}