namespace TinyHelpers.Threading;

/// <summary>
/// Provides a lock that can be used asynchronously.
/// </summary>
public class AsyncLock : IDisposable
{
    private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

    /// <summary>
    /// Asyncronously waits for the lock to become available.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An awaitable task.</returns>
    public Task<AsyncLock> LockAsync(CancellationToken cancellationToken = default)
    {
        return LockAsync(Timeout.Infinite, cancellationToken);
    }

    /// <summary>
    /// Asyncronously waits for the lock to become available, with a specific timeout in milliseconds.
    /// </summary>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="timeoutInMilliseconds"/> is a negative number other than -1 milliseconds, which represents
    /// an infinite timeout.
    /// </exception>
    /// <returns>An awaitable task.</returns>
    public async Task<AsyncLock> LockAsync(int timeoutInMilliseconds, CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(timeoutInMilliseconds, cancellationToken).ConfigureAwait(false);
        return this;
    }

    /// <summary>
    /// Asyncronously waits for the lock to become available, using a <see cref="TimeSpan"/> to specify a timeout.
    /// </summary>
    /// <param name="timeout">A <see cref="TimeSpan"/> that represents the maximum time to wait</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An awaitable task.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="timeout"/> is a negative number other than -1 milliseconds, which represents
    /// an infinite timeout -or- timeout is greater than <see cref="int.MaxValue"/>.
    /// </exception>
    public async Task<AsyncLock> LockAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(timeout, cancellationToken).ConfigureAwait(false);
        return this;
    }

    /// <summary>
    /// Releases the lock.
    /// </summary>
    public void Dispose()
    {
        if (semaphoreSlim.CurrentCount == 0)
        {
            semaphoreSlim.Release();
        }

        GC.SuppressFinalize(this);
    }
}
