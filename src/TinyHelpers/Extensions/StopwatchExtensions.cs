using System.Diagnostics;

namespace TinyHelpers.Extensions;

public static class StopwatchExtensions
{
    public static TimeSpan GetElapsedAndRestart(this Stopwatch stopwatch)
    {
        stopwatch.Stop();
        var result = stopwatch.Elapsed;

        stopwatch.Restart();

        return result;
    }
}