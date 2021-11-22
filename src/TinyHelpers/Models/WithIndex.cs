namespace TinyHelpers;

public readonly struct WithIndex<T> where T : class
{
    public T? Value { get; }

    public int Index { get; }

    internal WithIndex(T? value, int index)
        => (Value, Index) = (value, index);

    public void Deconstruct(out T? value, out int index)
        => (value, index) = (Value, Index);
}
