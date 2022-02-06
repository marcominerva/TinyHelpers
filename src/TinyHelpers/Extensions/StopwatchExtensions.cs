using System.Diagnostics;

namespace TinyHelpers.Extensions;

public static class StopwatchExtensions
{
    public static TimeSpan GetElapsedAndRestart(this Stopwatch sw)
    {
        sw.Stop();
        var result = sw.Elapsed;

        sw.Restart();

        return result;
    }
}