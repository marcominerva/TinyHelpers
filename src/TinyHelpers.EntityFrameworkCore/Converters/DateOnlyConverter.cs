using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

#if NET6_0 || NET7_0
public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
            dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
            dateTime => DateOnly.FromDateTime(dateTime))
    {
    }
}
#endif
