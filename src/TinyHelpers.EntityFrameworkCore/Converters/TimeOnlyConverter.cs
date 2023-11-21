using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

#if NET6_0 || NET7_0
public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyConverter() : base(
            timeOnly => timeOnly.ToTimeSpan(),
            timeSpan => TimeOnly.FromTimeSpan(timeSpan))
    {
    }
}
#endif