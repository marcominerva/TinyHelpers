using System;
using System.Threading;
using System.Threading.Tasks;

namespace TinyHelpers.Threading
{
    public class AsyncLock : IDisposable
    {
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

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
        }
    }
}
