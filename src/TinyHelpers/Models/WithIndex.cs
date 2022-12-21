namespace TinyHelpers;

/// <summary>
/// Represents a generic object with its relative index.
/// </summary>
/// <typeparam name="T">The internal type of the object represented.</typeparam>
public readonly struct WithIndex<T> where T : class
{
    /// <summary>
    /// Gets or sets the value of the object.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Gets or sets the index of the object.
    /// </summary>
    public int Index { get; }
        
    internal WithIndex(T? value, int index)
        => (Value, Index) = (value, index);

    /// <summary>
    /// Separates value and index of the instance.
    /// </summary>
    /// <param name="value">The value of the instance.</param>
    /// <param name="index">An <code>int</code> representing the index of the value of the instance.</param>
    public void Deconstruct(out T? value, out int index)
        => (value, index) = (Value, Index);
}
