using TinyHelpers.Enums;

namespace TinyHelpers.Extensions;

/// <summary>
/// Provides extension methods for types that implement <see cref="IComparable{T}"/>.
/// </summary>
public static class IComparableExtensions
{
    /// <summary>
    /// Determines whether the specified value is between the given lower and upper bounds based on the specified boundary type.
    /// </summary>
    /// <typeparam name="T">The type of the value, which must be a value type that implements <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="lowerValue">The lower bound to compare against.</param>
    /// <param name="upperValue">The upper bound to compare against.</param>
    /// <param name="boundaryType">The type of boundary checking to perform. Defaults to <see cref="BoundaryType.Inclusive"/>.</param>
    /// <param name="comparer">An optional comparer to use for comparison. If not provided, the default comparer for the type is used.</param>
    /// <returns>
    /// <see langword="true"/> if the value is between the lower and upper bounds according to the specified boundary type; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsBetween<T>(this T value, T lowerValue, T upperValue, BoundaryType boundaryType = BoundaryType.Inclusive, Comparer<T>? comparer = default) where T : struct, IComparable<T>
    {
        var valueComparer = comparer ?? Comparer<T>.Default;

        return boundaryType switch
        {
            BoundaryType.Inclusive => valueComparer.Compare(value, lowerValue) >= 0 && valueComparer.Compare(value, upperValue) <= 0,
            BoundaryType.LowerInclusive or BoundaryType.UpperExclusive => valueComparer.Compare(value, lowerValue) >= 0 && valueComparer.Compare(value, upperValue) < 0,
            BoundaryType.UpperInclusive or BoundaryType.LowerExclusive => valueComparer.Compare(value, lowerValue) > 0 && valueComparer.Compare(value, upperValue) <= 0,
            BoundaryType.Exclusive => valueComparer.Compare(value, lowerValue) > 0 && valueComparer.Compare(value, upperValue) < 0,
            _ => throw new ArgumentOutOfRangeException(nameof(boundaryType), boundaryType, "Invalid boundary type specified.")
        };
    }

    /// <summary>
    /// Determines whether the specified value is between the given lower and upper bounds based on the specified boundary type.
    /// </summary>
    /// <typeparam name="T">The type of the value, which must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="sender">The value to compare.</param>
    /// <param name="minimumValue">The lower bound to compare against.</param>
    /// <param name="maximumValue">The upper bound to compare against.</param>
    /// <param name="boundaryType">The type of boundary checking to perform. Defaults to <see cref="BoundaryType.Inclusive"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value is between the lower and upper bounds according to the specified boundary type; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsBetween<T>(this IComparable<T> sender, T minimumValue, T maximumValue, BoundaryType boundaryType = BoundaryType.Inclusive)
    {
        return boundaryType switch
        {
            BoundaryType.Inclusive => sender.CompareTo(minimumValue) >= 0 && sender.CompareTo(maximumValue) <= 0,
            BoundaryType.LowerInclusive or BoundaryType.UpperExclusive => sender.CompareTo(minimumValue) >= 0 && sender.CompareTo(maximumValue) < 0,
            BoundaryType.UpperInclusive or BoundaryType.LowerExclusive => sender.CompareTo(minimumValue) > 0 && sender.CompareTo(maximumValue) <= 0,
            BoundaryType.Exclusive => sender.CompareTo(minimumValue) > 0 && sender.CompareTo(maximumValue) < 0,
            _ => throw new ArgumentOutOfRangeException(nameof(boundaryType), boundaryType, "Invalid boundary type specified.")
        };
    }
}
