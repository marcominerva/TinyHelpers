namespace TinyHelpers.Threading;

/// <summary>
/// Represents the result of an asynchronous lock operation.
/// </summary>
public readonly struct LockResult
{
    /// <summary>
    /// Gets the <seealso cref="Threading.AsyncLock"/> object if succesfully acquired.
    /// </summary>
    public AsyncLock? AsyncLock { get; }

    /// <summary>
    /// Gets a boolean indicating if the lock was acquired or not.
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
