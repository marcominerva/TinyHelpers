using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace TinyHelpers.Extensions;

/// <summary>
/// Extension methods for better manage collections
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
    /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare keys.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from the source sequence.</returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
        => source.GroupBy(keySelector, comparer).Select(x => x.First());
#endif

    /// <summary>
    /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data in the <code>source</code>.</typeparam>
    /// <param name="source">The sequence on whose elements apply the <code>action</code> to.</param>
    /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the List<T>.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the <code>action</code> on each element of <code>source</code>.</returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }

        return source;
    }

    /// <summary>
    /// Asynchronously performs the specified action on each element of the <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the data in the <code>source</code>.</typeparam>
    /// <param name="source">The sequence on whose elements apply the <code>action</code> to.</param>
    /// <param name="action">An asynchronous delegate that is invoked once per element in the data source.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An <see cref="IEnumerable{T}" /> whose elements are the result of invoking the <code>action</code> on each element of <code>source</code>.</returns>
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
    /// <returns>An <see cref="IEnumerable{T}" /> whose elements are the result of invoking the transform function on each element of <code>source</code>.</returns>
    /// <exception cref="ArgumentNullException"><code>source</code> is <code>null</code></exception>    
    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selector, CancellationToken cancellationToken = default)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var result = new List<TResult>();
        foreach (var item in source)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result.Add(await selector(item).ConfigureAwait(false));
        }

        return result;
    }

    /// <summary>
    /// Creates an <see cref="IEnumerable{T}"/> from an <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IAsyncEnumerable{T}"/> to create a <see cref="IEnumerable{T}"/> from.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence.</returns>
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
    /// Remove from a collection all the elements that meets the criteria specified by the <code>predicate</code>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}"/> to remove elements from.</param>
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
    /// Creates an <see cref="IEnumerable{T}"/> of <see cref="TinyHelpers.WithIndex{T}"/> elements from an <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to create an <see cref="IEnumerable{T}"/> of <see cref="TinyHelpers.WithIndex{T}"/> elements from.</param>    
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="TinyHelpers.WithIndex{T}"/> elements that contains projected elements from the input sequence.</returns>
    public static IEnumerable<WithIndex<TSource>> WithIndex<TSource>(this IEnumerable<TSource> source) where TSource : class
        => source.Select((item, index) => new WithIndex<TSource>(item, index));

    /// <summary>
    /// Gets a value that indicates whether this sequence is empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>    
    /// <returns>true if the <code>source</code> is empty; otherwise, false.</returns>
    public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        => !source.Any();

    /// <summary>
    /// Gets a value that indicates whether this sequence is not empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>    
    /// <returns>true if the <code>source</code> is not empty; otherwise, false.</returns>
    public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> source)
        => source.Any();

    /// <summary>
    /// Gets a value that indicates whether this sequence is <code>null</code> or empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>    
    /// <returns>true if the <code>source</code> is <code>null</code> or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty<TSource>([NotNullWhen(false)] this IEnumerable<TSource>? source)
        => !source?.Any() ?? true;

    /// <summary>
    /// Gets a value that indicates whether this sequence is not <code>null</code> or empty.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>    
    /// <returns>true if the <code>source</code> is not <code>null</code> and not empty; otherwise, false.</returns>
    public static bool IsNotNullOrEmpty<TSource>([NotNullWhen(true)] this IEnumerable<TSource>? source)
        => source?.Any() ?? false;

    /// <summary>
    /// Gets a value that indicates whether this sequence has items.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>    
    /// <returns>true if the <code>source</code> has any item; otherwise, false.</returns>
    public static bool HasItems<TSource>([NotNullWhen(true)] this IEnumerable<TSource>? source)
        => source.IsNotNullOrEmpty();

    /// <summary>
    /// Gets the number of elements in the sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>    
    /// <param name="predicate">The delegate function that defines the conditions of the elements to consider for the count.</param>
    /// <returns>An <see cref="int"/> representing the number of elements that meet the criteria defined by the <code>predicate</code> function, if not <code>null</code>; the number of elements in the <code>source</code>, otherwise.</returns>
    public static int GetCount<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate = null)
        => (predicate is null ? source?.Count() : source?.Count(predicate)) ?? 0;

    /// <summary>
    /// Gets the number of elements in the sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>    
    /// <param name="predicate">The delegate function that defines the conditions of the elements to consider for the count.</param>
    /// <returns>A <see cref="long"/> representing the number of elements that meet the criteria defined by the <code>predicate</code> function, if not <code>null</code>; the number of elements in the <code>source</code>, otherwise.</returns>
    public static long GetLongCount<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool>? predicate = null)
        => (predicate is null ? source?.LongCount() : source?.LongCount(predicate)) ?? 0;

    /// <summary>
    /// Filters a sequence of values based on a function, if given.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>    
    /// <param name="condition">A flag indicating whether a <code>predicate</code> function filter should be applied</param>
    /// <param name="predicate">The delegate function that defines the conditions of the elements to filter.</param>
    /// <returns>An <see cref="IEnumerable{T}"/>that contains elements from the input sequence that satisfy the condition specified by <code>predicate</code>, if given; the unfiltered <code>source</code>, otherwise.</returns>
    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        => condition ? source.Where(predicate) : source;

    /// <summary>
    /// Filters a sequence of values based on a lambda expression, if given.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <code>source</code>.</typeparam>
    /// <param name="source">The <see cref="IQueryable{T}"/> to check.</param>    
    /// <param name="condition">A flag indicating whether a <code>predicate</code> expression filter should be applied</param>
    /// <param name="predicate">The lambda expression that defines the conditions of the elements to filter.</param>
    /// <returns>An <see cref="IQueryable{T}"/>that contains elements from the input sequence that satisfy the condition specified by <code>predicate</code>, if given; the unfiltered <code>source</code>, otherwise.</returns>
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        => condition ? source.Where(predicate) : source;
}
