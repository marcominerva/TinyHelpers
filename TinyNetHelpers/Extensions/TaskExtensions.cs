using System;
using System.Threading;
using System.Threading.Tasks;

namespace TinyNetHelpers.Extensions
{
    public static class TaskExtensions
    {
        public static async Task TimeoutAfterAsync(this Task task, TimeSpan timeout)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token)).ConfigureAwait(false);
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                await task.ConfigureAwait(false);  // Very important in order to propagate exceptions
            }
            else
            {
                throw new TimeoutException("The operation has timed out");
            }
        }

        public static async Task<TResult> TimeoutAfterAsync<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token)).ConfigureAwait(false);
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                return await task.ConfigureAwait(false);  // Very important in order to propagate exceptions
            }
            else
            {
                throw new TimeoutException("The operation has timed out");
            }
        }
    }
}
