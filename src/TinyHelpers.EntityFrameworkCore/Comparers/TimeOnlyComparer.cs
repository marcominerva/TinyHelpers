using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TinyHelpers.EntityFrameworkCore.Comparers;

#if NET6_0 || NET7_0
public class TimeOnlyComparer : ValueComparer<TimeOnly>
{
    public TimeOnlyComparer() : base(
        (t1, t2) => t1.Ticks == t2.Ticks,
        t => t.GetHashCode())
    {
    }
}
#endif
