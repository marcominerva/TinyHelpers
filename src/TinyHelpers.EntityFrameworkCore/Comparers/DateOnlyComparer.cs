using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TinyHelpers.EntityFrameworkCore.Comparers;

#if NET6_0 || NET7_0
public class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer() : base(
        (d1, d2) => d1.DayNumber == d2.DayNumber,
        d => d.GetHashCode())
    {
    }
}
#endif