using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyNetHelpers.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }

            return source;
        }

        public static IEnumerable<WithIndex<T>> WithIndex<T>(this IEnumerable<T> enumerable) where T : class
            => enumerable.Select((item, index) => new WithIndex<T>(item, index));
    }

    public readonly struct WithIndex<T> where T : class
    {
        public T? Value { get; }

        public int Index { get; }

        public WithIndex(T? value, int index)
            => (Value, Index) = (value, index);

        public void Deconstruct(out T? value, out int index)
            => (value, index) = (Value, Index);
    }
}
