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

        public static async ValueTask<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            var list = new List<TSource>();
            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                list.Add(item);
            }

            return list;
        }

        public static IEnumerable<WithIndex<T>> WithIndex<T>(this IEnumerable<T> source) where T : class
            => source.Select((item, index) => new WithIndex<T>(item, index));
    }
}
