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
    /// <returns>An awaitable task.</returns>
    public async Task<AsyncLock> LockAsync()
    {
        await semaphoreSlim.WaitAsync().ConfigureAwait(false);
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
