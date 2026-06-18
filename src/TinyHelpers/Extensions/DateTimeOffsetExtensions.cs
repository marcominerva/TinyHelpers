namespace TinyHelpers.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="DateTimeOffset"/> type.
/// </summary>
/// <seealso cref="DateTimeOffset"/>
public static class DateTimeOffsetExtensions
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Constructs a <see cref="DateOnly"/> object that is set to the date part of the specified <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> object to extract the date part from.</param>
    /// <param name="zone">The optional <see cref="TimeZoneInfo" /> to apply to the resulting <see cref="DateOnly"/> value; UTC is used when no zone is specified.</param>
    /// <returns>A <see cref="DateOnly" /> value for the date in the selected time zone.</returns>
    /// <seealso cref="DateTimeOffset"/>
    /// <seealso cref="DateOnly"/>
    /// <seealso cref="TimeZoneInfo"/>
    public static DateOnly ToDateOnly(this DateTimeOffset dateTimeOffset, TimeZoneInfo? zone = null)
    {
        var inTargetZone = TimeZoneInfo.ConvertTime(dateTimeOffset, zone ?? TimeZoneInfo.Utc);
        return DateOnly.FromDateTime(inTargetZone.Date);
    }

    /// <summary>
    /// Constructs a <see cref="TimeOnly"/> object from a <see cref="DateTimeOffset"/> representing the time of the day in this <see cref="DateTimeOffset"/> object.
    /// </summary>
    /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> object to extract the time of the day from.</param>
    /// <param name="zone">The optional <see cref="TimeZoneInfo" /> to apply to the resulting <see cref="TimeOnly"/> value; UTC is used when no zone is specified.</param>
    /// <returns>A <see cref="TimeOnly" /> value for the time in the selected time zone.</returns>
    /// <seealso cref="DateTimeOffset"/>
    /// <seealso cref="TimeOnly"/>
    /// <seealso cref="TimeZoneInfo"/>
    public static TimeOnly ToTimeOnly(this DateTimeOffset dateTimeOffset, TimeZoneInfo? zone = null)
    {
        var inTargetZone = TimeZoneInfo.ConvertTime(dateTimeOffset, zone ?? TimeZoneInfo.Utc);
        return TimeOnly.FromDateTime(inTargetZone.DateTime);
    }
#endif
}
