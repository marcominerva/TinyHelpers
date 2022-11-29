namespace TinyHelpers.Threading;

public class AsyncLock : IDisposable
{
    private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

    public async Task<AsyncLock> LockAsync()
    {
        await semaphoreSlim.WaitAsync().ConfigureAwait(false);
        return this;
    }

    public void Dispose()
    {
        if (semaphoreSlim.CurrentCount == 0)
        {
            semaphoreSlim.Release();
        }

        GC.SuppressFinalize(this);
    }
}
