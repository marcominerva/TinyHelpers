using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TinyHelpers.Extensions;

namespace TinyHelpers.EntityFrameworkCore.Converters;

public class StringArrayConverter : ValueConverter<IEnumerable<string>?, string?>
{
#pragma warning disable EF1001 // Internal EF Core API usage.

    public StringArrayConverter(string separator = ";") : base(
            list => list.HasItems() ? string.Join(separator, list!) : null,
            value => value.HasValue() ? value!.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) : Enumerable.Empty<string>(),
            convertsNulls: true)

#pragma warning restore EF1001 // Internal EF Core API usage.
    {
    }
}