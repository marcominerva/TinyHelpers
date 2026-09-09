using TinyHelpers.Threading;

namespace TinyHelpers.Tests.Threading;

public class AsyncLockTests
{
    [Fact]
    public async Task LockAsync_Uncontended_ReturnsSameLockAndDisposeReleasesIt()
    {
        using var sut = new AsyncLock();
        var cancellationToken = TestContext.Current.CancellationToken;

        var acquired = await sut.LockAsync(cancellationToken);
        acquired.Dispose();
        var result = await sut.LockAsync(0, cancellationToken);

        Assert.Same(sut, acquired);
        Assert.True(result.IsOwned);
        Assert.Same(sut, result.AsyncLock);
    }

    [Fact]
    public async Task LockAsync_ContendedZeroTimeout_ReturnsUnownedResultAndDeconstructs()
    {
        using var sut = new AsyncLock();
        var cancellationToken = TestContext.Current.CancellationToken;
        await sut.LockAsync(cancellationToken);

        var result = await sut.LockAsync(0, cancellationToken);
        var (isOwned, asyncLock) = result;

        Assert.False(isOwned);
        Assert.Null(asyncLock);
    }

    [Fact]
    public async Task LockAsync_TimeSpanOverload_SucceedsAndTimesOut()
    {
        using var sut = new AsyncLock();
        var cancellationToken = TestContext.Current.CancellationToken;
        var acquired = await sut.LockAsync(TimeSpan.Zero, cancellationToken);
        var timedOut = await sut.LockAsync(TimeSpan.Zero, cancellationToken);

        Assert.True(acquired.IsOwned);
        Assert.False(timedOut.IsOwned);
    }

    [Fact]
    public async Task LockAsync_CanceledToken_ThrowsWhenContended()
    {
        using var sut = new AsyncLock();
        await sut.LockAsync(TestContext.Current.CancellationToken);
        using var source = new CancellationTokenSource();
        source.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => sut.LockAsync(source.Token));
    }

    [Fact]
    public async Task LockAsync_InvalidTimeout_Throws()
    {
        using var sut = new AsyncLock();
        var cancellationToken = TestContext.Current.CancellationToken;

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.LockAsync(-2, cancellationToken));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            sut.LockAsync(TimeSpan.FromMilliseconds(-2), cancellationToken));
    }

    [Fact]
    public async Task DisposeAsync_ReleasesLockAndCanBeCalledRepeatedly()
    {
        var sut = new AsyncLock();
        var cancellationToken = TestContext.Current.CancellationToken;
        await sut.LockAsync(cancellationToken);

        await sut.DisposeAsync();
        await sut.DisposeAsync();
        var result = await sut.LockAsync(0, cancellationToken);

        Assert.True(result.IsOwned);
        result.AsyncLock?.Dispose();
    }
}
