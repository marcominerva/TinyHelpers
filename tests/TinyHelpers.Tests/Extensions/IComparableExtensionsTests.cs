using TinyHelpers.Enums;
using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class IComparableExtensionsTests
{
    [Theory]
    [InlineData(5, 1, 10, BoundaryType.Inclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.Inclusive, true)]
    [InlineData(10, 1, 10, BoundaryType.Inclusive, true)]
    [InlineData(0, 1, 10, BoundaryType.Inclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.Inclusive, false)]
    [InlineData(5, 1, 10, BoundaryType.Exclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.Exclusive, false)]
    [InlineData(10, 1, 10, BoundaryType.Exclusive, false)]
    [InlineData(0, 1, 10, BoundaryType.Exclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.Exclusive, false)]
    [InlineData(5, 1, 10, BoundaryType.LowerInclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.LowerInclusive, true)]
    [InlineData(10, 1, 10, BoundaryType.LowerInclusive, false)]
    [InlineData(0, 1, 10, BoundaryType.LowerInclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.LowerInclusive, false)]
    [InlineData(5, 1, 10, BoundaryType.UpperInclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.UpperInclusive, false)]
    [InlineData(10, 1, 10, BoundaryType.UpperInclusive, true)]
    [InlineData(0, 1, 10, BoundaryType.UpperInclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.UpperInclusive, false)]
    [InlineData(5, 1, 10, BoundaryType.LowerExclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.LowerExclusive, false)]
    [InlineData(10, 1, 10, BoundaryType.LowerExclusive, true)]
    [InlineData(0, 1, 10, BoundaryType.LowerExclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.LowerExclusive, false)]
    [InlineData(3, 1, 10, BoundaryType.UpperExclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.UpperExclusive, true)]
    [InlineData(10, 1, 10, BoundaryType.UpperExclusive, false)]
    public void IsBetween_Struct_With_BoundaryType_Should_Return_Expected_Result(int value, int lowerValue, int upperValue, BoundaryType boundaryType, bool expected)
    {
        // Act
        var result = value.IsBetween(lowerValue, upperValue, boundaryType);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5, 1, 10, BoundaryType.Inclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.Inclusive, true)]
    [InlineData(10, 1, 10, BoundaryType.Inclusive, true)]
    [InlineData(0, 1, 10, BoundaryType.Inclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.Inclusive, false)]
    [InlineData(5, 1, 10, BoundaryType.Exclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.Exclusive, false)]
    [InlineData(10, 1, 10, BoundaryType.Exclusive, false)]
    [InlineData(0, 1, 10, BoundaryType.Exclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.Exclusive, false)]
    [InlineData(5, 1, 10, BoundaryType.LowerInclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.LowerInclusive, true)]
    [InlineData(10, 1, 10, BoundaryType.LowerInclusive, false)]
    [InlineData(0, 1, 10, BoundaryType.LowerInclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.LowerInclusive, false)]
    [InlineData(5, 1, 10, BoundaryType.UpperInclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.UpperInclusive, false)]
    [InlineData(10, 1, 10, BoundaryType.UpperInclusive, true)]
    [InlineData(0, 1, 10, BoundaryType.UpperInclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.UpperInclusive, false)]
    [InlineData(5, 1, 10, BoundaryType.LowerExclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.LowerExclusive, false)]
    [InlineData(10, 1, 10, BoundaryType.LowerExclusive, true)]
    [InlineData(0, 1, 10, BoundaryType.LowerExclusive, false)]
    [InlineData(11, 1, 10, BoundaryType.LowerExclusive, false)]
    [InlineData(3, 1, 10, BoundaryType.UpperExclusive, true)]
    [InlineData(1, 1, 10, BoundaryType.UpperExclusive, true)]
    [InlineData(10, 1, 10, BoundaryType.UpperExclusive, false)]
    public void IsBetween_IComparable_With_BoundaryType_Should_Return_Expected_Result(int value, int lowerValue, int upperValue, BoundaryType boundaryType, bool expected)
    {
        // Arrange
        IComparable<int> comparableValue = value;

        // Act
        var result = comparableValue.IsBetween(lowerValue, upperValue, boundaryType);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsBetween_Struct_With_Custom_Comparer_Should_Use_Comparer()
    {
        // Arrange
        var comparer = Comparer<int>.Create((x, y) => x.CompareTo(y));

        // Act
        var result = 5.IsBetween(1, 10, BoundaryType.Inclusive, comparer);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsBetween_Struct_With_Invalid_BoundaryType_Should_Throw_ArgumentOutOfRangeException()
    {
        // Arrange
        var invalidBoundaryType = (BoundaryType)999;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 5.IsBetween(1, 10, invalidBoundaryType));
    }

    [Fact]
    public void IsBetween_IComparable_With_Invalid_BoundaryType_Should_Throw_ArgumentOutOfRangeException()
    {
        // Arrange
        IComparable<int> value = 5;
        var invalidBoundaryType = (BoundaryType)999;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => value.IsBetween(1, 10, invalidBoundaryType));
    }

    [Theory]
    [InlineData("2023-01-15", "2023-01-01", "2023-01-31", BoundaryType.Inclusive, true)]
    [InlineData("2023-01-01", "2023-01-01", "2023-01-31", BoundaryType.Inclusive, true)]
    [InlineData("2023-01-31", "2023-01-01", "2023-01-31", BoundaryType.Inclusive, true)]
    [InlineData("2022-12-31", "2023-01-01", "2023-01-31", BoundaryType.Inclusive, false)]
    [InlineData("2023-02-01", "2023-01-01", "2023-01-31", BoundaryType.Inclusive, false)]
    [InlineData("2023-01-15", "2023-01-01", "2023-01-31", BoundaryType.Exclusive, true)]
    [InlineData("2023-01-01", "2023-01-01", "2023-01-31", BoundaryType.Exclusive, false)]
    [InlineData("2023-01-31", "2023-01-01", "2023-01-31", BoundaryType.Exclusive, false)]
    public void IsBetween_DateTime_Should_Work_Correctly(string valueStr, string lowerStr, string upperStr, BoundaryType boundaryType, bool expected)
    {
        // Arrange
        var value = DateTime.Parse(valueStr);
        var lower = DateTime.Parse(lowerStr);
        var upper = DateTime.Parse(upperStr);

        // Act
        var result = value.IsBetween(lower, upper, boundaryType);

        // Assert
        Assert.Equal(expected, result);
    }
}