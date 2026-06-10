using Microsoft.EntityFrameworkCore;

namespace System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Marks a property or field as being stored using a vector column.
/// </summary>
/// <remarks>
/// This attribute exists so Entity Framework Core models can express vector storage intent directly on the
/// entity member, which keeps the mapping close to the domain type and avoids scattering provider-specific
/// column configuration across <see cref="ModelBuilder" /> code.
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class VectorAttribute : ColumnAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VectorAttribute" /> class with the default column name.
    /// </summary>
    /// <param name="size">The vector dimension to encode into the column type.</param>
    public VectorAttribute(int size = 1536)
    {
        TypeName = $"vector({size})";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VectorAttribute" /> class using a specific column name.
    /// </summary>
    /// <param name="name">The database column name.</param>
    /// <param name="size">The vector dimension to encode into the column type.</param>
    public VectorAttribute(string name, int size = 1536) : base(name)
    {
        TypeName = $"vector({size})";
    }
}