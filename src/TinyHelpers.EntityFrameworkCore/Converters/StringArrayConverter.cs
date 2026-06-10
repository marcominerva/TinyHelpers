using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TinyHelpers.Extensions;

namespace TinyHelpers.EntityFrameworkCore.Converters;

#pragma warning disable EF1001 // Internal EF Core API usage.
public class StringArrayConverter(string separator = ";") : ValueConverter<IEnumerable<string>?, string?>(
            list => list.HasItems() ? string.Join(separator, list!) : null,
            value => value.HasValue() ? value!.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) : Enumerable.Empty<string>(),
            convertsNulls: true);
#pragma warning restore EF1001 // Internal EF Core API usage.
