using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyHelpers.EntityFrameworkCore.Comparers;
using TinyHelpers.EntityFrameworkCore.Converters;

namespace TinyHelpers.EntityFrameworkCore.Extensions;

/// <summary>
/// Provides <see cref="PropertyBuilder" /> helpers for converting common value shapes into database-friendly
/// representations.
/// </summary>
/// <remarks>
/// These helpers keep the conversion and comparison pieces together so model configuration can state the persistence
/// intent once, while Entity Framework Core still receives the metadata it needs for materialization and change tracking.
/// </remarks>
public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Configures a property to be stored as JSON while preserving change tracking semantics for equivalent
    /// serialized values.
    /// </summary>
    /// <typeparam name="T">The CLR type.</typeparam>
    /// <param name="propertyBuilder">The property builder to configure.</param>
    /// <param name="jsonSerializerOptions">Optional serializer settings used for persistence.</param>
    /// <param name="useUtcDate">Whether to normalize dates to UTC during serialization.</param>
    /// <param name="serializeEnumAsString">Whether enums should be emitted as strings instead of numbers.</param>
    /// <returns>The same <see cref="PropertyBuilder{TProperty}" /> instance so calls can be chained.</returns>
    /// <remarks>
    /// Use this when a value object or small object graph belongs to the entity and should live in a single text column.
    /// The method wires both <see cref="JsonStringConverter{T}" /> and <see cref="JsonStringComparer{T}" /> so EF Core
    /// compares the serialized payload instead of object references.
    /// </remarks>
    public static PropertyBuilder<T?> HasJsonConversion<T>(this PropertyBuilder<T?> propertyBuilder, JsonSerializerOptions? jsonSerializerOptions = null, bool useUtcDate = false, bool serializeEnumAsString = false)
    {
        jsonSerializerOptions ??= new(JsonOptions.Default);

        if (useUtcDate)
        {
            jsonSerializerOptions.Converters.Add(new Json.Serialization.UtcDateTimeConverter());
        }

        if (serializeEnumAsString)
        {
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        var converter = new JsonStringConverter<T>(jsonSerializerOptions);
        var comparer = new JsonStringComparer<T>(jsonSerializerOptions);

        propertyBuilder.HasConversion(converter, comparer);

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a string sequence to be stored as a single delimited column.
    /// </summary>
    /// <param name="propertyBuilder">The property builder to configure.</param>
    /// <param name="separator">The delimiter used when joining and splitting values.</param>
    /// <returns>The same <see cref="PropertyBuilder{TProperty}" /> instance so calls can be chained.</returns>
    /// <remarks>
    /// This mapping is intended for small collections that should stay on the entity row and do not need independent
    /// relational querying. It also registers <see cref="StringArrayComparer" /> so tracking is based on sequence content.
    /// </remarks>
    public static PropertyBuilder<IEnumerable<string>> HasArrayConversion(this PropertyBuilder<IEnumerable<string>> propertyBuilder, string separator = ";")
    {
        var converter = new StringArrayConverter(separator);
        var comparer = new StringArrayComparer();

        propertyBuilder.HasConversion(converter!, comparer);
        return propertyBuilder;
    }

    /// <summary>
    /// Configures a string array to be stored as a single delimited column.
    /// </summary>
    /// <param name="propertyBuilder">The property builder to configure.</param>
    /// <param name="separator">The delimiter used when joining and splitting values.</param>
    /// <returns>The same <see cref="PropertyBuilder{TProperty}" /> instance so calls can be chained.</returns>
    /// <remarks>
    /// This mapping is intended for small arrays that should stay on the entity row and do not need independent
    /// relational querying. It also registers <see cref="StringArrayComparer" /> so tracking is based on sequence content.
    /// </remarks>
    public static PropertyBuilder<string[]> HasArrayConversion(this PropertyBuilder<string[]> propertyBuilder, string separator = ";")
    {
        var converter = new StringArrayConverter(separator);
        var comparer = new StringArrayComparer();

        propertyBuilder.HasConversion(converter, comparer);
        return propertyBuilder;
    }

    /// <summary>
    /// Maps a property to a vector column type with the specified dimension.
    /// </summary>
    /// <param name="propertyBuilder">The property builder to configure.</param>
    /// <param name="size">The vector dimension encoded into the database type name.</param>
    /// <returns>The same <see cref="PropertyBuilder" /> instance so calls can be chained.</returns>
    /// <remarks>
    /// Use this when the database provider understands vector columns and the property represents embeddings or other
    /// vector-search data.
    /// </remarks>
    public static PropertyBuilder IsVector(this PropertyBuilder propertyBuilder, int size = 1536)
        => propertyBuilder.HasColumnType($"vector({size})");

    /// <summary>
    /// Maps a property to a vector column type with the specified dimension.
    /// </summary>
    /// <typeparam name="T">The modeled CLR type.</typeparam>
    /// <param name="propertyBuilder">The property builder to configure.</param>
    /// <param name="size">The vector dimension encoded into the database type name.</param>
    /// <returns>The same <see cref="PropertyBuilder{TProperty}" /> instance so calls can be chained.</returns>
    /// <remarks>
    /// Use this when the database provider understands vector columns and the property represents embeddings or other
    /// vector-search data.
    /// </remarks>
    public static PropertyBuilder<T> IsVector<T>(this PropertyBuilder<T> propertyBuilder, int size = 1536)
        => propertyBuilder.HasColumnType($"vector({size})");
}
