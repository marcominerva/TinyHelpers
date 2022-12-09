using FluentAssertions;
using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("Marco", false, false)]
    [InlineData("Marco", true, false)]
    [InlineData("Marco", false, true)]
    [InlineData("Marco", true, true)]
    [InlineData("", true, false)]
    [InlineData("", true, true)]
    [InlineData(" ", true, false)]
    [InlineData(" ", true, true)]
    [InlineData(" ", false, false)]
    public void HasValue_ValidString_Should_Return_True(string? input, bool allowEmptyString, bool whiteSpaceAsEmpty)
    {
        // Arrange

        // Act
        var hasValue = input.HasValue(allowEmptyString, whiteSpaceAsEmpty);

        // Assert
        hasValue.Should().BeTrue();
        //Assert.True(hasValue);
    }

    [Theory]
    [InlineData("", false, false)]
    [InlineData("", false, true)]
    [InlineData(" ", false, true)]
    [InlineData(null, false, false)]
    [InlineData(null, true, false)]
    [InlineData(null, false, true)]
    [InlineData(null, true, true)]
    public void HasValue_InvalidString_Should_Return_False(string? input, bool allowEmptyString, bool whiteSpaceAsEmpty)
    {
        // Arrange

        // Act
        var hasValue = input.HasValue(allowEmptyString, whiteSpaceAsEmpty);

        // Assert
        hasValue.Should().BeFalse();
        //Assert.False(hasValue);
    }
}