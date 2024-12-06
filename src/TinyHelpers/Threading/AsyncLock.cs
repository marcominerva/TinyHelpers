namespace TinyHelpers.Threading;

/// <summary>
/// Provides a lock that can be used asynchronously.
/// </summary>
public class AsyncLock : IDisposable
{
    private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

    /// <summary>
    /// Asynchronously waits for the lock to become available.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An awaitable task.</returns>
    public async Task<AsyncLock> LockAsync(CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);
        return this;
    }

    /// <summary>
    /// Asynchronously waits for the lock to become available, with a specific timeout in milliseconds.
    /// </summary>
    /// <param name="timeoutInMilliseconds">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An awaitable task that returns a <see cref="LockResult"/> indicating whether the lock was acquired or not and the lock itself.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="timeoutInMilliseconds"/> is a negative number other than -1 milliseconds, which represents
    /// an infinite timeout.
    /// </exception>
    public async Task<LockResult> LockAsync(int timeoutInMilliseconds, CancellationToken cancellationToken = default)
    {
        var isOwned = await semaphoreSlim.WaitAsync(timeoutInMilliseconds, cancellationToken).ConfigureAwait(false);
        return new LockResult(isOwned ? this : null, isOwned);
    }

    /// <summary>
    /// Asynchronously waits for the lock to become available, using a <see cref="TimeSpan"/> to specify a timeout.
    /// </summary>
    /// <param name="timeout">A <see cref="TimeSpan"/> that represents the maximum time to wait</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An awaitable task that returns a <see cref="LockResult"/> indicating whether the lock was acquired or not and the lock itself.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="timeout"/> is a negative number other than -1 milliseconds, which represents
    /// an infinite timeout or timeout is greater than <see cref="int.MaxValue"/>.
    /// </exception>
    public async Task<LockResult> LockAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        var isOwned = await semaphoreSlim.WaitAsync(timeout, cancellationToken).ConfigureAwait(false);
        return new LockResult(isOwned ? this : null, isOwned);
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
