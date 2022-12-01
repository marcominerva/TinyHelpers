using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace TinyHelpers.Extensions;

public static class CollectionExtensions
{
#if NETSTANDARD2_0
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
        => source.GroupBy(keySelector, comparer).Select(x => x.First());
#endif

    public static IEnumerable<T>? ForEach<T>(this IEnumerable<T>? source, Action<T> action)
    {
        if (source is not null)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        return source;
    }

    public static async Task<IEnumerable<TSource>?> ForEachAsync<TSource>(this IEnumerable<TSource>? source, Func<TSource, Task> action, CancellationToken cancellationToken = default)
    {
        if (source is not null)
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
        if (source is not null)
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

    public static async ValueTask<IEnumerable<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
    {
        var list = new List<TSource>();
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            list.Add(item);
        }

        return list;
    }

    public static void Remove<TSource>(this ICollection<TSource> collection, Func<TSource, bool> predicate)
    {
        for (var i = collection.Count - 1; i >= 0; i--)
        {
            var element = collection.ElementAt(i);
            if (predicate(element))
            {
                collection.Remove(element);
            }
        }
    }

    public static IEnumerable<WithIndex<TSource>> WithIndex<TSource>(this IEnumerable<TSource> source) where TSource : class
        => source.Select((item, index) => new WithIndex<TSource>(item, index));

    public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        => !source.Any();

    public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> source)
        => source.Any();

    public static bool IsNullOrEmpty<TSource>([NotNullWhen(false)] this IEnumerable<TSource>? source)
        => !source?.Any() ?? true;

    public static bool IsNotNullOrEmpty<TSource>([NotNullWhen(true)] this IEnumerable<TSource>? source)
        => source?.Any() ?? false;

    public static bool HasItems<TSource>([NotNullWhen(true)] this IEnumerable<TSource>? source)
        => source.IsNotNullOrEmpty();

    public static int GetCount<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate = null)
        => (predicate is null ? source?.Count() : source?.Count(predicate)) ?? 0;

    public static long GetLongCount<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate = null)
        => (predicate is null ? source?.LongCount() : source?.LongCount(predicate)) ?? 0;

    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        => condition ? source.Where(predicate) : source;

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        => condition ? source.Where(predicate) : source;
}
