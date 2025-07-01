namespace TinyHelpers.Enums;

/// <summary>
/// Specifies the type of boundary checking to perform when comparing values.
/// </summary>
public enum BoundaryType
{
    /// <summary>
    /// Include both lower and upper bounds in the comparison.
    /// </summary>
    Inclusive = 0,

    /// <summary>
    /// Include the lower bound but exclude the upper bound in the comparison.
    /// </summary>
    LowerInclusive = 1,

    /// <summary>
    /// Exclude the lower bound but include the upper bound in the comparison.
    /// </summary>
    UpperInclusive = 2,

    /// <summary>
    /// Exclude both lower and upper bounds in the comparison.
    /// </summary>
    Exclusive = 3,

    /// <summary>
    /// Exclude the lower bound but include the upper bound in the comparison (same as <see cref="UpperInclusive"/>).
    /// </summary>
    LowerExclusive = 4,

    /// <summary>
    /// Include the lower bound and exclude the upper bound in the comparison (same as <see cref="LowerInclusive"/>).
    /// </summary>
    UpperExclusive = 5
}
