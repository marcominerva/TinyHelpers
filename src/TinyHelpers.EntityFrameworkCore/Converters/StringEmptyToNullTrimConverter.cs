using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

public class StringEmptyToNullTrimConverter : ValueConverter<string?, string?>
{
    public StringEmptyToNullTrimConverter() : base(
            value => string.IsNullOrWhiteSpace(value) ? null : value.Trim(),
            value => value)
    {
    }
}
