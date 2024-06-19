using FluentAssertions;
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
        output.Should().Be(result);
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
        output.Should().Be(result);
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
        output.Should().NotBe(result);
    }
}
