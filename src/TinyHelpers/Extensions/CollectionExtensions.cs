namespace TinyHelpers.Extensions;

public static class CollectionExtensions
{
#if NETSTANDARD2_0
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
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

    public static async Task<IEnumerable<T>?> ForEachAsync<T>(this IEnumerable<T>? source, Func<T, Task> action, CancellationToken cancellationToken = default)
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

    public static async ValueTask<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
    {
        var list = new List<TSource>();
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            list.Add(item);
        }

        return list;
    }

    public static void Remove<T>(this ICollection<T> collection, Func<T, bool> predicate)
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

    public static IEnumerable<WithIndex<T>> WithIndex<T>(this IEnumerable<T> source) where T : class
        => source.Select((item, index) => new WithIndex<T>(item, index));

    public static bool IsEmpty<T>(this IEnumerable<T> source)
        => !source.Any();

    public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        => source.Any();

    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        => !source?.Any() ?? true;

    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source)
        => source?.Any() ?? false;

    public static int GetCount<T>(this IEnumerable<T>? source, Func<T, bool>? predicate = null)
        => source?.Count(predicate!) ?? 0;

    public static long GetLongCount<T>(this IEnumerable<T>? source, Func<T, bool>? predicate = null)
        => source?.LongCount(predicate!) ?? 0;
}
