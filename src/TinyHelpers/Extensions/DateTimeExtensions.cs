namespace TinyHelpers.Extensions;

/// <summary>
/// Extension methods for <see cref="DateTime"/> type.
/// </summary>
/// <seealso cref="DateTime"/>
/// <seealso cref="DateOnly"/>
/// <seealso cref="TimeOnly"/>
public static class DateTimeExtensions
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Constructs a <see cref="DateOnly"/> object that is set to the date part of the specified <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> object to extract the date part from.</param>
    /// <returns>A <see cref="TimeOnly"/> object representing date of the day specified in the <paramref name="dateTime"/> object.</returns>
    /// <seealso cref="DateTime"/>
    /// <seealso cref="DateOnly"/>
    public static DateOnly ToDateOnly(this DateTime dateTime)
        => DateOnly.FromDateTime(dateTime);

    /// <summary>
    /// Constructs a <see cref="TimeOnly"/> object from a <see cref="DateTime"/> representing the time of the day in this <see cref="DateTime"/> object.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> object to extract the time of the day from.</param>
    /// <returns>A <see cref="TimeOnly"/> object representing time of the day specified in the <paramref name="dateTime"/> object.</returns>
    /// <seealso cref="DateTime"/>
    /// <seealso cref="TimeOnly"/>
    public static TimeOnly ToTimeOnly(this DateTime dateTime)
        => TimeOnly.FromDateTime(dateTime);

    /// <summary>
    /// Constructs a <see cref="TimeOnly"/> object from a <see cref="TimeSpan"/> representing the time elapsed since midnight.
    /// </summary>
    /// <param name="timeSpan">The time interval measured since midnight. This value has to be positive and not exceeding the time of the day.</param>
    /// <returns>A <see cref="TimeOnly"/> object representing the time elapsed since midnight using the <paramref name="timeSpan"/> value.</returns>
    /// <seealso cref="DateTime"/>
    /// <seealso cref="TimeOnly"/>
    public static TimeOnly ToTimeOnly(this TimeSpan timeSpan)
        => TimeOnly.FromTimeSpan(timeSpan);
#endif
}
