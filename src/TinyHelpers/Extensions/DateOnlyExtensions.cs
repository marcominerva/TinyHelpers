namespace TinyHelpers.Extensions;

#if NET6_0_OR_GREATER
/// <summary>
/// Contains extension methods for the <see cref="DateOnly"/> type.
/// </summary>
/// <seealso cref="DateOnly"/>
public static class DateOnlyExtensions
{
    /// <summary>
    /// Converts a <see cref="DateOnly"/> value to a <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="dateOnly">The <see cref="DateOnly"/> value to convert.</param>
    /// <param name="zone">The optional <see cref="TimeZoneInfo"/> to apply to the resulting <see cref="DateTimeOffset"/> value.</param>
    /// <returns>The converted <see cref="DateTimeOffset"/> value.</returns>
    /// <seealso cref="DateOnly"/>
    /// <seealso cref="DateTimeOffset"/>
    /// <seealso cref="TimeZoneInfo"/>
    public static DateTimeOffset ToDateTimeOffset(this DateOnly dateOnly, TimeZoneInfo? zone = null)
    {
        var dateTime = dateOnly.ToDateTime(TimeOnly.MinValue);
        return new DateTimeOffset(dateTime, zone?.GetUtcOffset(dateTime) ?? TimeSpan.Zero);
    }
}
#endif