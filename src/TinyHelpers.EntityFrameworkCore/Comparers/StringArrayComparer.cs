using Microsoft.EntityFrameworkCore.ChangeTracking;
using TinyHelpers.EntityFrameworkCore.Converters;

namespace TinyHelpers.EntityFrameworkCore.Comparers;

/// <summary>
/// Compares string sequences by sequence content so Entity Framework Core tracks collection changes deterministically.
/// </summary>
/// <remarks>
/// This comparer is paired with <see cref="StringArrayConverter" /> to store a string collection in a single
/// column while still letting Entity Framework Core detect real content changes instead of treating the collection instance as
/// the only signal.
/// </remarks>
public class StringArrayComparer() : ValueComparer<IEnumerable<string>>(
            (list1, list2) => (list1 ?? Array.Empty<string>()).SequenceEqual(list2 ?? Array.Empty<string>()),
            list => GetSequenceHashCode(list),
            list => list == null ? Array.Empty<string>() : list.ToArray())
{
    private static int GetSequenceHashCode(IEnumerable<string>? list)
    {
        if (list is null)
        {
            return 0;
        }

        var hash = new HashCode();

        foreach (var item in list)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }
}
