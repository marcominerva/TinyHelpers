using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class RangeExtensionsTests
{
    [Theory]
    [InlineData(1, 3, new[] { 1, 2, 3 })]
    [InlineData(3, 1, new[] { 3, 2, 1 })]
    [InlineData(2, 2, new[] { 2 })]
    public void RangeEnumerator_EnumeratesInclusiveRange(int start, int end, int[] expected)
    {
        var values = new List<int>();
        foreach (var value in new Range(start, end))
        {
            values.Add(value);
        }

        Assert.Equal(expected, values);
    }

    [Fact]
    public void RangeEnumerator_FromEndStart_EnumeratesDescendingValues()
    {
        var enumerator = new Range(^3, 1).GetEnumerator();
        var values = new List<int>();
        while (enumerator.MoveNext())
        {
            values.Add(enumerator.Current);
        }

        Assert.Equal([3, 2, 1], values);
    }
}
