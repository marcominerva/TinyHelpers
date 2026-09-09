using System.Diagnostics;
using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class StopwatchExtensionsTests
{
    [Fact]
    public void GetElapsedAndRestart_ReturnsPreviousElapsedAndRestartsStopwatch()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        stopwatch.Stop();
        var expected = stopwatch.Elapsed;

        var result = stopwatch.GetElapsedAndRestart();

        Assert.Equal(expected, result);
        Assert.True(stopwatch.IsRunning);
    }
}
