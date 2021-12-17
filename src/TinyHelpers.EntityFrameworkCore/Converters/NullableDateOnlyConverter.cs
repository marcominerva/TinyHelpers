using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
{
    public NullableDateOnlyConverter() : base(
        dateOnly => dateOnly == null
            ? null
            : new DateTime?(dateOnly.Value.ToDateTime(TimeOnly.MinValue)),
        dateTime => dateTime == null
            ? null
            : new DateOnly?(DateOnly.FromDateTime(dateTime.Value)))
    {
    }
}
