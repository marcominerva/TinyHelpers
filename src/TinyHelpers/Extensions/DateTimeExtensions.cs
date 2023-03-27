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

    public static string ToFriendlyDateTimeString(this DateTime Date)
    {
        return FriendlyDate(Date) + " @ " + Date.ToString("t").ToLower();
    }

    public static string ToFriendlyShortDateString(this DateTime Date)
    {
        return $"{Date.ToString("MMM dd")}, {Date.Year}";
    }

    public static string ToFriendlyDateString(this DateTime Date)
    {
        return FriendlyDate(Date);
    }

    static string FriendlyDate(DateTime date)
    {
        string FormattedDate = "";
        if (date.Date == DateTime.Today)
        {
            FormattedDate = "Today";
        }
        else if (date.Date == DateTime.Today.AddDays(-1))
        {
            FormattedDate = "Yesterday";
        }
        else if (date.Date > DateTime.Today.AddDays(-6))
        {
            // *** Show the Day of the week
            FormattedDate = date.ToString("dddd").ToString();
        }
        else
        {
            FormattedDate = date.ToString("MMMM dd, yyyy");
        }

        return FormattedDate;
    }
}
