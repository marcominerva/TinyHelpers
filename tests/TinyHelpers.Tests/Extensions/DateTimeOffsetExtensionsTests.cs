using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class DateTimeOffsetExtensionsTests
{
    [Fact]
    public void DateTimeOffsetConversions_DefaultToUtc()
    {
        var value = new DateTimeOffset(2026, 9, 10, 1, 30, 0, TimeSpan.FromHours(3));

        Assert.Equal(new DateOnly(2026, 9, 9), value.ToDateOnly());
        Assert.Equal(new TimeOnly(22, 30), value.ToTimeOnly());
    }

    [Fact]
    public void DateTimeOffsetConversions_WithZone_UseTargetZone()
    {
        var zone = TimeZoneInfo.CreateCustomTimeZone("Test", TimeSpan.FromHours(-4), "Test", "Test");
        var value = new DateTimeOffset(2026, 9, 9, 2, 30, 0, TimeSpan.Zero);

        Assert.Equal(new DateOnly(2026, 9, 8), value.ToDateOnly(zone));
        Assert.Equal(new TimeOnly(22, 30), value.ToTimeOnly(zone));
    }
}
