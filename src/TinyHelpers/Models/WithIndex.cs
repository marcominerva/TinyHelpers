using TinyHelpers.Extensions;

namespace TinyHelpers;

/// <summary>
/// Represents a generic object with its relative index within a collection.
/// </summary>
/// <typeparam name="T">The type of the object.</typeparam>
/// <seealso cref="CollectionExtensions.WithIndex{TSource}(IEnumerable{TSource})"/>
public readonly struct WithIndex<T> where T : class
{
    /// <summary>
    /// Gets the value of the object.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Gets the index of the object.
    /// </summary>
    public int Index { get; }

    internal WithIndex(T? value, int index)
    {
        (Value, Index) = (value, index);
    }

    /// <summary>
    /// Separates value and index of the instance.
    /// </summary>
    /// <param name="value">The value of the instance.</param>
    /// <param name="index">The index of the value of the instance.</param>
    public void Deconstruct(out T? value, out int index)
        => (value, index) = (Value, Index);
}
