namespace TinyHelpers.Enums;

#if NET9_0_OR_GREATER
/// <summary>
/// Specifies the version of the <see cref="Guid"/>.
/// </summary>
public enum GuidVersion
{
    /// <summary>
    /// Represents a version 4 <see cref="Guid"/>.
    /// </summary>
    Version4,

    /// <summary>
    /// Represents a version 7 <see cref="Guid"/>.
    /// </summary>
    Version7
}
#endif
