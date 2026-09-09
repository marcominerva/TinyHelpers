using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class TimeSpanExtensionsTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(452967890000)]
    [InlineData(863999999999)]
    public void TimeSpanToTimeOnly_ValidTimeOfDay_ReturnsExpected(long ticks)
    {
        Assert.Equal(TimeOnly.FromTimeSpan(TimeSpan.FromTicks(ticks)), TimeSpan.FromTicks(ticks).ToTimeOnly());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(864000000000)]
    public void TimeSpanToTimeOnly_OutOfRange_Throws(long ticks)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TimeSpan.FromTicks(ticks).ToTimeOnly());
    }
}
