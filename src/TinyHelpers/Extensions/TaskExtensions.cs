namespace TinyHelpers.Extensions;

/// <summary>
/// Extension methods for managing tasks
/// </summary>
public static class TaskExtensions
{
#if NETSTANDARD2_0

    /// <summary>
    /// Gets a <see cref="Task"/> that will complete when this <see cref="Task"/> completes or when the specified timeout expires.
    /// </summary>
    /// <param name="task">The <see cref="Task"/> to await for.</param>
    /// <param name="timeout">The timeout after which the <see cref="Task"/> should be faulted with a <see cref="TimeoutException"/> if it hasn't otherwise completed.</param>
    /// <returns>The <see cref="Task"/> representing the asynchronous wait.</returns>
    /// <exception cref="TimeoutException">The timeout is reached before the <see cref="Task"/> has completed.</exception>
    public static async Task WaitAsync(this Task task, TimeSpan timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token)).ConfigureAwait(false);
        if (completedTask == task)
        {
            timeoutCancellationTokenSource.Cancel();
            await task.ConfigureAwait(false);
        }
        else
        {
            throw new TimeoutException();
        }
    }

    /// <summary>
    /// Gets a <see cref="Task"/> that will complete when this <see cref="Task"/> completes or when the specified timeout expires.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="task">The <see cref="Task{T}"/> to await for.</param>
    /// <param name="timeout">The timeout after which the <see cref="Task{T}"/> should be faulted with a <see cref="TimeoutException"/> if it hasn't otherwise completed.</param>
    /// <returns>The <see cref="Task{T}"/> representing the asynchronous wait.</returns>
    /// <exception cref="TimeoutException">The timeout is reached before the <see cref="Task"/> has completed.</exception>
    public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token)).ConfigureAwait(false);
        if (completedTask == task)
        {
            timeoutCancellationTokenSource.Cancel();
            return await task.ConfigureAwait(false);
        }
        else
        {
            throw new TimeoutException();
        }
    }
#endif
}
