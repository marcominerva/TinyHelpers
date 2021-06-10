using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TinyHelpers.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
            => source.GroupBy(keySelector, comparer).Select(x => x.First());

        public static IEnumerable<T>? ForEach<T>(this IEnumerable<T>? source, Action<T> action)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }

            return source;
        }

        public static async Task<IEnumerable<T>?> ForEachAsync<T>(this IEnumerable<T>? source, Func<T, Task> action, CancellationToken cancellationToken = default)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await action.Invoke(item).ConfigureAwait(false);
                }
            }

            return source;
        }

        public static async Task<IEnumerable<TResult>?> SelectAsync<TSource, TResult>(this IEnumerable<TSource>? source, Func<TSource, Task<TResult>> asyncSelector, CancellationToken cancellationToken = default)
        {
            if (source != null)
            {
                var result = new List<TResult>();
                foreach (var item in source)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    result.Add(await asyncSelector(item).ConfigureAwait(false));
                }

                return result;
            }

            return null;
        }

        public static IEnumerable<WithIndex<T>> WithIndex<T>(this IEnumerable<T> source) where T : class
            => source.Select((item, index) => new WithIndex<T>(item, index));
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
