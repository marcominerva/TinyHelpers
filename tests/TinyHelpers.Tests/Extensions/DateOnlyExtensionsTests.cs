using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class DateOnlyExtensionsTests
{
    [Fact]
    public void Should_Return_True1()
    {
        // Arrange
        var input = new DateOnly(2020, 1, 1);
        var result = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);

        // Act
        var output = input.ToDateTimeOffset();

        // Assert
        Assert.Equal(result, output);
    }

    [Fact]
    public void Should_Return_True2()
    {
        // Arrange
        var input = new DateOnly(2023, 6, 12);
        var result = new DateTimeOffset(2023, 6, 12, 0, 0, 0, TimeSpan.Zero);

        // Act
        var output = input.ToDateTimeOffset();

        // Assert
        Assert.Equal(result, output);
    }

    [Fact]
    public void Should_Return_False()
    {
        // Arrange
        var input = new DateOnly(2020, 1, 1);
        var result = new DateTimeOffset(2020, 1, 1, 4, 0, 0, TimeSpan.Zero);

        // Act
        var output = input.ToDateTimeOffset();

        // Assert
        Assert.NotEqual(result, output);
    }

    [Fact]
    public void DateOnlyToDateTimeOffset_WithoutZone_UsesMidnightUtc()
    {
        var result = new DateOnly(2026, 9, 9).ToDateTimeOffset();

        Assert.Equal(new DateTimeOffset(2026, 9, 9, 0, 0, 0, TimeSpan.Zero), result);
    }

    [Fact]
    public void DateOnlyToDateTimeOffset_WithZone_UsesZoneOffset()
    {
        var zone = TimeZoneInfo.CreateCustomTimeZone("Test", TimeSpan.FromHours(3), "Test", "Test");

        var result = new DateOnly(2026, 9, 9).ToDateTimeOffset(zone);

        Assert.Equal(TimeSpan.FromHours(3), result.Offset);
        Assert.Equal(new DateTime(2026, 9, 9), result.Date);
    }
}
