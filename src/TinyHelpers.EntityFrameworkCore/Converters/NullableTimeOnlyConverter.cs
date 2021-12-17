using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

public class NullableTimeOnlyConverter : ValueConverter<TimeOnly?, TimeSpan?>
{
    public NullableTimeOnlyConverter() : base(
        timeOnly => timeOnly == null
            ? null
            : new TimeSpan?(timeOnly.Value.ToTimeSpan()),
        timeSpan => timeSpan == null
            ? null
            : new TimeOnly?(TimeOnly.FromTimeSpan(timeSpan.Value)))
    {
    }
}
