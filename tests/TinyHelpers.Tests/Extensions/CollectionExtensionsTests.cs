using TinyHelpers.Extensions;

namespace TinyHelpers.Tests.Extensions;

public class CollectionExtensionsTests
{
    [Fact]
    public void Chunk_ExactMultipleChunkSize_ReturnsEqualChunks()
    {
        // Arrange
        var source = Enumerable.Range(1, 6);
        int chunkSize = 3;

        // Act
        var result = source.ChunkBySize(chunkSize).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { 1, 2, 3 }, result[0].ToArray());
        Assert.Equal(new[] { 4, 5, 6 }, result[1].ToArray());
    }

    [Fact]
    public void Chunk_ChunkSizeNotDivisible_ReturnsLastChunkSmaller()
    {
        // Arrange
        var source = Enumerable.Range(1, 7);
        int chunkSize = 3;

        // Act
        var result = source.ChunkBySize(chunkSize).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 1, 2, 3 }, result[0].ToArray());
        Assert.Equal(new[] { 4, 5, 6 }, result[1].ToArray());
        Assert.Equal(new[] { 7 }, result[2].ToArray());
    }

    [Fact]
    public void GetLongCount_PredicateAlwaysTrue_ReturnsTotalCount()
    {
        // Arrange
        var source = Enumerable.Range(1, 10);

        // Act
        long result = source.GetLongCount(x => true);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void GetLongCount_PredicateAlwaysFalse_ReturnsZero()
    {
        // Arrange
        var source = Enumerable.Range(1, 10);

        // Act
        long result = source.GetLongCount(x => x > 10);

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
}
