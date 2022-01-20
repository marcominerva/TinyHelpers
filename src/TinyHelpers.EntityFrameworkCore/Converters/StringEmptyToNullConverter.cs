using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

public class StringEmptyToNullConverter : ValueConverter<string?, string?>
{
    public StringEmptyToNullConverter() : base(
            value => string.IsNullOrWhiteSpace(value) ? null : value,
            value => value)
    {
    }
}
