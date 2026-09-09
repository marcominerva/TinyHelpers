using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class CollectionExtensionsTests
{

    [Fact]
    public void Chunk_ExactMultipleChunkSize_ReturnsEqualChunks()
    {
        // Arrange
        var source = Enumerable.Range(1, 6);
        var chunkSize = 3;

        // Act
        var result = source.Chunk(chunkSize).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 1, 2, 3 }, result[0]);
        Assert.Equal(new[] { 4, 5, 6 }, result[1]);
    }

    [Fact]
    public void Chunk_ChunkSizeNotDivisible_ReturnsLastChunkSmaller()
    {
        // Arrange
        var source = Enumerable.Range(1, 7);
        var chunkSize = 3;

        // Act
        var result = source.Chunk(chunkSize).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 1, 2, 3 }, result[0]);
        Assert.Equal(new[] { 4, 5, 6 }, result[1]);
        Assert.Equal(new[] { 7 }, result[2]);
    }

    [Fact]
    public void GetLongCount_PredicateAlwaysTrue_ReturnsTotalCount()
    {
        // Arrange
        var source = Enumerable.Range(1, 10);

        // Act
        var result = source.GetLongCount(x => true);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void GetLongCount_PredicateAlwaysFalse_ReturnsZero()
    {
        // Arrange
        var source = Enumerable.Range(1, 10);

        // Act
        var result = source.GetLongCount(x => x > 10);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Remove_MatchingElements_RemovesCorrectElements()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        collection.Remove(x => x % 2 == 0);

        // Assert
        Assert.Equal(new[] { 1, 3, 5 }, collection);
    }

    [Fact]
    public void PerformAction_ForEach_ReturnDesiredResult()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3, 4, 5 };
        var modifiedList = new List<int>();  // Local variable for storing result

        // Act
        collection.ForEach(x => modifiedList.Add(x * 5));

        // Assert
        Assert.Equal(new[] { 5, 10, 15, 20, 25 }, modifiedList);
    }

    [Fact]
    public void List_IsEmpty_ReturnTrue()
    {
        var collection = new List<int>();

        Assert.True(collection.IsEmpty());
    }

    [Fact]
    public void EmptyIfNull_EnumerableAndQueryable_ReturnEmptySequences()
    {
        IEnumerable<int>? enumerable = null;
        IQueryable<int>? queryable = null;

        Assert.Empty(enumerable.EmptyIfNull());
        Assert.Empty(queryable.EmptyIfNull());
    }

    [Fact]
    public void EmptyIfNull_NonNull_ReturnsOriginalInstances()
    {
        IEnumerable<int> enumerable = [1];
        IQueryable<int> queryable = new[] { 1 }.AsQueryable();

        Assert.Same(enumerable, enumerable.EmptyIfNull());
        Assert.Same(queryable, queryable.EmptyIfNull());
    }

    [Fact]
    public void ForEach_ReturnsSourceAndInvokesActionInOrder()
    {
        IEnumerable<int> source = [1, 2, 3];
        var visited = new List<int>();

        var result = source.ForEach(visited.Add);

        Assert.Same(source, result);
        Assert.Equal(source, visited);
    }

    [Fact]
    public async Task ForEachAsync_ReturnsSourceAndInvokesActionInOrder()
    {
        IEnumerable<int> source = [1, 2, 3];
        var visited = new List<int>();

        var result = await source.ForEachAsync(item =>
        {
            visited.Add(item);
            return Task.CompletedTask;
        }, TestContext.Current.CancellationToken);

        Assert.Same(source, result);
        Assert.Equal(source, visited);
    }

    [Fact]
    public async Task ForEachAsync_CanceledToken_ThrowsBeforeInvokingAction()
    {
        using var source = new CancellationTokenSource();
        source.Cancel();
        var invoked = false;

        await Assert.ThrowsAsync<OperationCanceledException>(() => new[] { 1 }.ForEachAsync(_ =>
        {
            invoked = true;
            return Task.CompletedTask;
        }, source.Token));
        Assert.False(invoked);
    }

    [Fact]
    public async Task SelectAsync_ProjectsInOrder()
    {
        var result = await new[] { 1, 2, 3 }.SelectAsync(
            value => Task.FromResult(value * 2), TestContext.Current.CancellationToken);

        Assert.Equal([2, 4, 6], result);
    }

    [Fact]
    public async Task SelectAsync_CanceledToken_Throws()
    {
        using var source = new CancellationTokenSource();
        source.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => new[] { 1 }.SelectAsync(Task.FromResult, source.Token));
    }

    [Fact]
    public async Task ToListAsync_EnumeratesAsyncSource()
    {
        var result = await TinyHelpers.Extensions.CollectionExtensions.ToListAsync(
            Values(), TestContext.Current.CancellationToken);

        Assert.Equal([1, 2, 3], result);

        static async IAsyncEnumerable<int> Values()
        {
            yield return 1;
            await Task.Yield();
            yield return 2;
            yield return 3;
        }
    }

    [Fact]
    public async Task ToListAsync_CanceledToken_Throws()
    {
        using var source = new CancellationTokenSource();
        source.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await TinyHelpers.Extensions.CollectionExtensions.ToListAsync(Values(source.Token), source.Token));

        static async IAsyncEnumerable<int> Values(
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            yield return 1;
        }
    }

    [Fact]
    public void Remove_NoMatches_PreservesCollection()
    {
        ICollection<int> source = [1, 3];

        source.Remove(value => value % 2 == 0);

        Assert.Equal([1, 3], source);
    }

#pragma warning disable CS0618
    [Fact]
    public void WithIndex_Enumerable_ProjectsValueAndIndexAndDeconstructs()
    {
        var result = new[] { "a", "b" }.WithIndex().ToArray();
        var (value, index) = result[1];

        Assert.Equal("a", result[0].Value);
        Assert.Equal(0, result[0].Index);
        Assert.Equal("b", value);
        Assert.Equal(1, index);
    }

    [Fact]
    public void WithIndex_Queryable_ProjectsValueAndIndex()
    {
        var result = new[] { "a", "b" }.AsQueryable().WithIndex().ToArray();

        Assert.Equal([0, 1], result.Select(item => item.Index));
        Assert.Equal(["a", "b"], result.Select(item => item.Value));
    }
#pragma warning restore CS0618

    [Theory]
    [InlineData(false, true, false, false)]
    [InlineData(true, false, true, true)]
    public void CollectionStateMethods_ReturnExpectedValues(bool populated, bool isEmpty, bool isNotEmpty, bool hasItems)
    {
        IEnumerable<int> source = populated ? [1] : [];

        Assert.Equal(isEmpty, source.IsEmpty());
        Assert.Equal(isNotEmpty, source.IsNotEmpty());
        Assert.Equal(hasItems, source.IsNotNullOrEmpty());
        Assert.Equal(hasItems, source.HasItems());
        Assert.Equal(isEmpty, source.IsNullOrEmpty());
    }

    [Fact]
    public void NullCollectionStateAndCounts_ReturnEmptyResults()
    {
        IEnumerable<int>? source = null;

        Assert.True(source.IsNullOrEmpty());
        Assert.False(source.IsNotNullOrEmpty());
        Assert.False(source.HasItems());
        Assert.Equal(0, source.GetCount());
        Assert.Equal(0, source.GetLongCount(value => true));
    }

    [Fact]
    public void Counts_WithAndWithoutPredicate_ReturnExpectedValues()
    {
        IEnumerable<int> source = [1, 2, 3, 4];

        Assert.Equal(4, source.GetCount());
        Assert.Equal(2, source.GetCount(value => value % 2 == 0));
        Assert.Equal(4, source.GetLongCount());
        Assert.Equal(2, source.GetLongCount(value => value > 2));
    }

    [Theory]
    [InlineData(true, new[] { 2, 4 })]
    [InlineData(false, new[] { 1, 2, 3, 4 })]
    public void WhereIf_Enumerable_AppliesPredicateConditionally(bool condition, int[] expected)
    {
        Assert.Equal(expected, new[] { 1, 2, 3, 4 }.WhereIf(condition, value => value % 2 == 0));
    }

    [Theory]
    [InlineData(true, new[] { 3, 4 })]
    [InlineData(false, new[] { 1, 2, 3, 4 })]
    public void WhereIf_Queryable_AppliesPredicateConditionally(bool condition, int[] expected)
    {
        Assert.Equal(expected, new[] { 1, 2, 3, 4 }.AsQueryable().WhereIf(condition, value => value > 2));
    }
}
