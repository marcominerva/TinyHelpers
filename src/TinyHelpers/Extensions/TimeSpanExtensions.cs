namespace TinyHelpers.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="TimeSpan"/> type.
/// </summary>
/// <seealso cref="TimeSpan"/>
public static class TimeSpanExtensions
{
#if NET6_0_OR_GREATER
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
