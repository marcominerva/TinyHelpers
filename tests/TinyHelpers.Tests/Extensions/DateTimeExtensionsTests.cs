using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void DateTimeConversions_ReturnDateAndTimeParts()
    {
        var value = new DateTime(2026, 9, 9, 10, 23, 45, 123);

        Assert.Equal(new DateOnly(2026, 9, 9), value.ToDateOnly());
        Assert.Equal(new TimeOnly(10, 23, 45, 123), value.ToTimeOnly());
    }
}
