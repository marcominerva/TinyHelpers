using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TinyHelpers.EntityFrameworkCore.Comparers;

public class StringArrayComparer : ValueComparer<IEnumerable<string>>
{
    public StringArrayComparer() : base(
        (list1, list2) => (list1 ?? Array.Empty<string>()).SequenceEqual(list2 ?? Array.Empty<string>()),
        list => list == null ? 0 : list.GetHashCode())
    {
    }
}
