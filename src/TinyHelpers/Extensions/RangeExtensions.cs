namespace TinyHelpers.Extensions;

#if NET6_0_OR_GREATER
/// <summary>
/// Contains extension methods for the <see cref="Range"/> type.
/// </summary>
/// <seealso cref="Range"/>
public static class RangeExtensions
{
    /// <summary>
    /// Returns an enumerator that iterates through the range of integers defined by the specified <see cref="Range"/>.
    /// </summary>
    /// <param name="range">The range of integers to iterate over.</param>
    /// <returns>An enumerator that can be used to iterate through the range of integers.</returns>
    public static IEnumerator<int> GetEnumerator(this Range range)
    {
        if (range.Start.IsFromEnd || range.Start.Value >= range.End.Value)
        {
            for (var i = range.Start.Value; i >= range.End.Value; i--)
            {
                yield return i;
            }
        }
        else
        {
            for (var i = range.Start.Value; i <= range.End.Value; i++)
            {
                yield return i;
            }
        }
    }
}
#endif
