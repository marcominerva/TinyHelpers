using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TinyHelpers.Extensions;

namespace TinyHelpers.EntityFrameworkCore.Converters;

/// <summary>
/// Converts a string sequence to a delimiter-separated value so it can be persisted in a single database column.
/// </summary>
/// <param name="separator">The delimiter used to serialize and parse the string sequence.</param>
/// <remarks>/// This converter keeps simple list-shaped data close to the entity without requiring a join table. It is a
/// pragmatic mapping for cases where the collection is small, text-based, and does not need independent
/// relational querying.
/// </remarks>
#pragma warning disable EF1001 // Internal EF Core API usage.
public class StringArrayConverter(string separator = ";") : ValueConverter<IEnumerable<string>?, string?>(
        list => list.HasItems() ? string.Join(separator, list!) : null,
        value => value.HasValue() ? value!.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) : Enumerable.Empty<string>(),
        convertsNulls: true);
#pragma warning restore EF1001 // Internal EF Core API usage.
