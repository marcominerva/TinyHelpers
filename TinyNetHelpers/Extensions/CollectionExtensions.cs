using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public static async Task<IEnumerable<T>> ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action, CancellationToken cancellationToken = default)
        {
            foreach (var item in source)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await action.Invoke(item).ConfigureAwait(false);
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

        internal WithIndex(T? value, int index)
            => (Value, Index) = (value, index);

        public void Deconstruct(out T? value, out int index)
            => (value, index) = (Value, Index);
    }
}
