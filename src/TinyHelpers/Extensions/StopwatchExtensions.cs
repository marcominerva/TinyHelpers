using System.Diagnostics;

namespace TinyHelpers.Extensions;

/// <summary>
/// Contains extension methods for managing the <see cref="Stopwatch"/> object.
/// </summary>
/// <seealso cref="Stopwatch"/>
public static class StopwatchExtensions
{
    /// <summary>
    /// Gets the total elapsed time measured by the current instance and restart the instance.
    /// </summary>
    /// <param name="stopwatch">The current instance to restart.</param>
    /// <returns>A read-only <see cref="TimeSpan"/> representing the total elapsed time measured by the current instance.</returns>
    public static TimeSpan GetElapsedAndRestart(this Stopwatch stopwatch)
    {
        stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;

        stopwatch.Restart();

        return elapsed;
    }
}