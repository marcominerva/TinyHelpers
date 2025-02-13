using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace TinyHelpers.Extensions;

/// <summary>
/// Contains extension methods for collections.
/// </summary>
public static class CollectionExtensions
{
#if NETSTANDARD2_0
    /// <summary>
    /// Returns distinct elements from a sequence according to a specified key selector function and using a specified comparer to compare keys.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
    /// <param name="source">The sequence to remove duplicate elements from.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
    /// <returns>An <see cref="IEnumerable{TSource}"/> that contains distinct elements from the source sequence.</returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
        => source.GroupBy(keySelector, comparer).Select(x => x.First());

    /// <summary>
    /// Splits the source collection into chunks of a specified size.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source collection.</typeparam>
    /// <param name="source">The source collection to split into chunks.</param>
    /// <param name="chunkSize">The size of each chunk.</param>
    /// <returns>An <see cref="IEnumerable{TSource}"/> where each element is an array of elements of the specified chunk size.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="chunkSize"/> is less than 1.</exception>
    public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int chunkSize)
    {
        if (chunkSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkSize), "Chunk size must be greater than 0");
        }

        return source
            .Select((value, index) => new { Index = index, Value = value })
            .GroupBy(x => x.Index / chunkSize)
            .Select(g => g.Select(x => x.Value).ToArray());
    }
#endif

    /// <summary>
    /// Returns the elements of an <see cref="IEnumerable{TSource}"/>, or an empty singleton collection if the sequence is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the <code>source</code>.</typeparam>
    /// <param name="source">The sequence to return a default value for if it is <see langword="null"/>.</param>
    /// <returns>An empty <see cref="IEnumerable{TSource}"/> if source is <see langword="null"/>; otherwise, <paramref name="source"/>.</returns>
    public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource>? source)
        => source ?? [];

    /// <summary>
    /// Returns the elements of an <see cref="IQueryable{TSource}"/>, or an empty singleton collection if the sequence is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the <code>source</code>.</typeparam>
    /// <param name="source">The sequence to return a default value for if it is <see langword="null"/>.</param>
    /// <returns>An empty <see cref="IQueryable{TSource}"/> if source is <see langword="null"/>; otherwise, <paramref name="source"/>.</returns>
    public static IQueryable<TSource> EmptyIfNull<TSource>(this IQueryable<TSource>? source)
        => source ?? Array.Empty<TSource>().AsQueryable();

    /// <summary>
    /// Performs the specified action on each element of the <see cref="IEnumerable{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the <code>source</code>.</typeparam>
    /// <param name="source">The sequence on whose elements apply the <code>action</code> to.</param>
    /// <param name="action">The <see cref="Action{TSource}"/> delegate to perform on each element of the collection.</param>
    /// <returns>An <see cref="IEnumerable{TSource}"/> whose elements are the result of invoking the <code>action</code> on each element of <code>source</code>.</returns>
    public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
    {
        foreach (var item in source)
        {
            action(item);
        }

        return source;
    }

    /// <summary>
    /// Asynchronously performs the specified action on each element of the <see cref="IEnumerable{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the <code>source</code>.</typeparam>
    /// <param name="source">The sequence on whose elements apply the <code>action</code> to.</param>
    /// <param name="action">An asynchronous delegate that is invoked once per element in the data source.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An <see cref="IEnumerable{TSource}" /> whose elements are the result of invoking the <code>action</code> on each element of <code>source</code>.</returns>
    public static async Task<IEnumerable<TSource>> ForEachAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task> action, CancellationToken cancellationToken = default)
    {
        foreach (var item in source)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await action.Invoke(item).ConfigureAwait(false);
        }

        return source;
    }

    /// <summary>
    /// Asynchronously projects each element of a sequence into a new form.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by <code>selector</code>.</typeparam>
    /// <param name="source">A sequence of values to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An <see cref="IEnumerable{TResult}" /> whose elements are the result of invoking the transform function on each element of <code>source</code>.</returns>
    /// <exception cref="ArgumentNullException"><code>source</code> is <code>null</code></exception>    
    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selector, CancellationToken cancellationToken = default)
    {
        var result = new List<TResult>();
        foreach (var item in source)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result.Add(await selector(item).ConfigureAwait(false));
        }

        return result;
    }

    /// <summary>
    /// Creates an <see cref="IEnumerable{TSource}"/> from an <see cref="IAsyncEnumerable{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IAsyncEnumerable{TSource}"/> to create a <see cref="IEnumerable{TSource}"/> from.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An <see cref="IEnumerable{TSource}"/> that contains elements from the input sequence.</returns>
    public static async ValueTask<IEnumerable<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
    {
        var list = new List<TSource>();
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            list.Add(item);
        }

        return list;
    }

    /// <summary>
    /// Removes from a collection all the elements that match the criteria specified by the <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="collection">The <see cref="ICollection{TSource}"/> to remove elements from.</param>
    /// <param name="predicate">The delegate function that defines the conditions of the elements to remove.</param>
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

    /// <summary>
    /// Creates an <see cref="IEnumerable{TSource}"/> of <see cref="TinyHelpers.WithIndex{TSource}"/> elements from an <see cref="IEnumerable{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to create an <see cref="IEnumerable{TSource}"/> of <see cref="TinyHelpers.WithIndex{TSource}"/> elements from.</param>
    /// <returns>An <see cref="IEnumerable{TSource}"/> of <see cref="TinyHelpers.WithIndex{TSource}"/> elements that contains projected elements from the input sequence.</returns>
    /// <seealso cref="TinyHelpers.WithIndex{TSource}"/>
#if NET9_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("The WithIndex() method is obsolete. Please use Index() instead.")]
#endif
    public static IEnumerable<WithIndex<TSource>> WithIndex<TSource>(this IEnumerable<TSource> source) where TSource : class
        => source.Select((item, index) => new WithIndex<TSource>(item, index));

    /// <summary>
    /// Creates an <see cref="IQueryable{TSource}"/> of <see cref="TinyHelpers.WithIndex{TSource}"/> elements from an <see cref="IQueryable{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IQueryable{TSource}"/> to create an <see cref="IQueryable{TSource}"/> of <see cref="TinyHelpers.WithIndex{TSource}"/> elements from.</param>
    /// <returns>An <see cref="IQueryable{T}"/> of <see cref="TinyHelpers.WithIndex{TSource}"/> elements that contains projected elements from the input sequence.</returns>
    /// <seealso cref="TinyHelpers.WithIndex{TSource}"/>
#if NET9_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("The WithIndex() method is obsolete. Please use Index() instead.")]
#endif
    public static IQueryable<WithIndex<TSource>> WithIndex<TSource>(this IQueryable<TSource> source) where TSource : class
        => source.Select((item, index) => new WithIndex<TSource>(item, index));

    /// <summary>
    /// Gets a value that indicates whether the <paramref name="source"/> collection is empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check.</param>    
    /// <returns><see langword="true"/> if <paramref name="source"/> is empty; otherwise, <see langword="false"/>.</returns>
    public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        => !source.Any();

    /// <summary>
    /// Gets a value that indicates whether the <paramref name="source"/> collection is not empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check.</param>    
    /// <returns><see langword="true"/> if <paramref name="source"/> is not empty; otherwise, <see langword="false"/>.</returns>
    public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> source)
        => source.Any();

    /// <summary>
    /// Gets a value that indicates whether the <paramref name="source"/> collection is <see langword="null"/> or empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check.</param>    
    /// <returns><see langword="true"/> if <paramref name="source"/> is <see langword="null"/> or empty; otherwise, <see langword="false"/>.</returns>
    public static bool IsNullOrEmpty<TSource>([NotNullWhen(false)] this IEnumerable<TSource>? source)
        => !source?.Any() ?? true;

    /// <summary>
    /// Gets a value that indicates whether the <paramref name="source"/> collection is not <see langword="null"/> or empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check.</param>    
    /// <returns><see langword="true"/> if the <paramref name="source"/> is not <see langword="null"/> and not empty; otherwise, <see langword="false"/>.</returns>
    public static bool IsNotNullOrEmpty<TSource>([NotNullWhen(true)] this IEnumerable<TSource>? source)
        => source?.Any() ?? false;

    /// <summary>
    /// Gets a value that indicates whether the <paramref name="source"/> collection contains items.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check.</param>    
    /// <returns><see langword="true"/> if the <paramref name="source"/> is not <see langword="null"/> and contains any item; otherwise, <see langword="false"/>.</returns>
    public static bool HasItems<TSource>([NotNullWhen(true)] this IEnumerable<TSource>? source)
        => source.IsNotNullOrEmpty();

    /// <summary>
    /// Gets the number of elements in the <paramref name="source"/> collection.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check.</param>    
    /// <param name="predicate">The delegate function that defines the conditions of the elements to consider for the count.</param>
    /// <returns>A <see cref="int"/> representing the number of elements that meet the criteria defined by the <paramref name="predicate"/> function, if not <see langword="null"/>; the number of elements in <paramref name="source"/>, otherwise.</returns>
    public static int GetCount<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate = null)
        => (predicate is null ? source?.Count() : source?.Count(predicate)) ?? 0;

    /// <summary>
    /// Gets the number of elements in the <paramref name="source"/> collection.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check.</param>    
    /// <param name="predicate">The delegate function that defines the conditions of the elements to consider for the count.</param>
    /// <returns>A <see cref="long"/> representing the number of elements that meet the criteria defined by the <paramref name="predicate"/> function, if not <see langword="null"/>; the number of elements in <paramref name="source"/>, otherwise.</returns>
    public static long GetLongCount<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate = null)
        => (predicate is null ? source?.LongCount() : source?.LongCount(predicate)) ?? 0;

    /// <summary>
    /// Filters a sequence of values based on a condition.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check.</param>    
    /// <param name="condition">A flag indicating whether the <paramref name="predicate"/> function filter should be applied.</param>
    /// <param name="predicate">The delegate function that defines the conditions of the elements to filter.</param>
    /// <returns>An <see cref="IEnumerable{TSource}"/> that contains elements from the input sequence that satisfy the condition specified by <paramref name="predicate"/>; the unfiltered <paramref name="source"/>, otherwise.</returns>
    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        => condition ? source.Where(predicate) : source;

    /// <summary>
    /// Filters a sequence of values based on a condition.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements.</typeparam>
    /// <param name="source">The <see cref="IQueryable{TSource}"/> to check.</param>    
    /// <param name="condition">A flag indicating whether the <paramref name="predicate"/> expression filter should be applied.</param>
    /// <param name="predicate">The lambda expression that defines the conditions of the elements to filter.</param>
    /// <returns>An <see cref="IQueryable{TSource}"/> that contains elements from the input sequence that satisfy the condition specified by <paramref name="predicate"/>; the unfiltered <paramref name="source"/>, otherwise.</returns>
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        => condition ? source.Where(predicate) : source;
}
