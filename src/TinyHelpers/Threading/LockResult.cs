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

    internal LockResult(AsyncLock? asyncLock, bool isOwned)
    {
        (AsyncLock, IsOwned) = (asyncLock, isOwned);
    }

    /// <summary>
    /// Deconstruct the <see cref="LockResult"/> object.
    /// </summary>
    /// <param name="asyncLock">The <seealso cref="Threading.AsyncLock"/> object.</param>
    /// <param name="isOwned">The boolean indicating if the lock was acquired or not.</param>
    public void Deconstruct(out AsyncLock? asyncLock, out bool isOwned)
        => (asyncLock, isOwned) = (AsyncLock, IsOwned);
}
