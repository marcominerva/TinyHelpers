namespace TinyHelpers.Threading;

/// <summary>
/// Represents the outcome of a timed <see cref="Threading.AsyncLock" /> acquisition attempt.
/// </summary>
/// <remarks>
/// Timed lock attempts need to distinguish between successful acquisition and timeout without throwing. This value
/// carries both the ownership flag and the lock instance that must be disposed when ownership is granted.
/// </remarks>
public readonly struct LockResult
{
    /// <summary>
    /// Gets the <see cref="Threading.AsyncLock" /> instance to dispose when the lock was acquired.
    /// </summary>
    public AsyncLock? AsyncLock { get; }

    /// <summary>
    /// Gets a value indicating whether the lock was acquired before the timeout elapsed.
    /// </summary>
    public bool IsOwned { get; }

    internal LockResult(bool isOwned, AsyncLock? asyncLock)
    {
        (IsOwned, AsyncLock) = (isOwned, asyncLock);
    }

    /// <summary>
    /// Deconstruct the <see cref="LockResult"/> object.
    /// </summary>
    /// <param name="isOwned">The boolean indicating if the lock was acquired or not.</param>
    /// <param name="asyncLock">The <seealso cref="Threading.AsyncLock"/> object.</param>
    public void Deconstruct(out bool isOwned, out AsyncLock? asyncLock)
        => (isOwned, asyncLock) = (IsOwned, AsyncLock);
}
