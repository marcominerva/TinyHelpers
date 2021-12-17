using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TinyHelpers.EntityFrameworkCore.Comparers;

public class NullableTimeOnlyComparer : ValueComparer<TimeOnly?>
{
    public NullableTimeOnlyComparer() : base(
        (t1, t2) => t1.GetValueOrDefault().Ticks == t2.GetValueOrDefault().Ticks,
        t => t.GetValueOrDefault().GetHashCode())
    {
    }
}