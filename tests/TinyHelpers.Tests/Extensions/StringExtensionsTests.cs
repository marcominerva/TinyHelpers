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
        Assert.True(hasValue);
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
        Assert.False(hasValue);
    }

    [Theory]
    [InlineData(@"\Welcome\world", @"\welcome", @"\hello", @"\hello\world")]
    [InlineData(@"\Welcome\world", @"\WELCOME", @"\hello", @"\hello\world")]
    [InlineData(@"\Welcome\world", @".*", @"\hello", @"\Welcome\world")]
    public void ReplaceIgnoreCase_Should_Replace_Ignoring_Case(string input, string pattern, string replacement, string expected)
    {
        // Arrange

        // Act
        var newString = input.ReplaceIgnoreCase(pattern, replacement);

        // Assert
        Assert.Equal(expected, newString);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("m", "M")]
    [InlineData("marco", "Marco")]
    [InlineData("mARCo", "MARCo")]
    [InlineData("42", "42")]
    public void FirstCharToUpper(string? input, string expected)
    {
        // Arrange

        // Act
        var newString = input!.FirstCharToUpper();

        // Assert
        Assert.Equal(expected, newString);
    }

    [Theory]
    [InlineData(null, null, true)]
    [InlineData("VALUE", "value", true)]
    [InlineData("value", "other", false)]
    public void EqualsIgnoreCase_ReturnsExpected(string? left, string? right, bool expected)
    {
        Assert.Equal(expected, left.EqualsIgnoreCase(right));
    }

    [Theory]
    [InlineData("PrefixValue", "prefix", true)]
    [InlineData("PrefixValue", "value", false)]
    [InlineData(null, "prefix", false)]
    public void StartsWithIgnoreCase_ReturnsExpected(string? input, string value, bool expected)
    {
        Assert.Equal(expected, input.StartsWithIgnoreCase(value));
    }

    [Theory]
    [InlineData("PrefixValue", "VALUE", true)]
    [InlineData("PrefixValue", "prefix", false)]
    [InlineData(null, "value", false)]
    public void EndsWithIgnoreCase_ReturnsExpected(string? input, string value, bool expected)
    {
        Assert.Equal(expected, input.EndsWithIgnoreCase(value));
    }

    [Theory]
    [InlineData("PrefixValue", "fixv", true)]
    [InlineData("PrefixValue", "other", false)]
    [InlineData(null, "value", false)]
    public void ContainsIgnoreCase_ReturnsExpected(string? input, string value, bool expected)
    {
        Assert.Equal(expected, input.ContainsIgnoreCase(value));
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("   ", null)]
    [InlineData(" value ", " value ")]
    public void GetValueOrDefault_WithoutFallback_ReturnsExpected(string? input, string? expected)
    {
        Assert.Equal(expected, input.GetValueOrDefault());
    }

    [Theory]
    [InlineData(null, "fallback", true, "fallback")]
    [InlineData("", "fallback", true, "fallback")]
    [InlineData(" ", "fallback", true, "fallback")]
    [InlineData(" ", "fallback", false, " ")]
    [InlineData("value", "fallback", false, "value")]
    public void GetValueOrDefault_WithOptions_ReturnsExpected(string? input, string? fallback, bool whitespaceAsEmpty, string? expected)
    {
        Assert.Equal(expected, input.GetValueOrDefault(fallback, whitespaceAsEmpty));
    }

    [Theory]
    [InlineData("value", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData(null, false)]
    public void HasValue_DefaultOverload_ReturnsExpected(string? input, bool expected)
    {
        Assert.Equal(expected, input.HasValue());
    }

    [Theory]
    [InlineData("", true, true)]
    [InlineData(" ", false, false)]
    [InlineData("value", false, true)]
    public void HasValue_AllowEmptyOverload_ReturnsExpected(string? input, bool allowEmpty, bool expected)
    {
        Assert.Equal(expected, input.HasValue(allowEmpty));
    }
}