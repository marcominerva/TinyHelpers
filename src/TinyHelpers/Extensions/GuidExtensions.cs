using System.Diagnostics.CodeAnalysis;
using TinyHelpers.Enums;

namespace TinyHelpers.Extensions;

/// <summary>
/// Contains extensions methods for the <see cref="Guid"/> type.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// Checks whether the given <see cref="Guid"/> is equals to <c>Guid.Empty</c>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/> is equals to <c>Guid.Empty</c>; otherwise, <see langword="false"/>.</returns>
    /// <seealso cref="Guid"/>
    public static bool IsEmpty(this Guid input)
        => input == Guid.Empty;

    /// <summary>
    /// Checks whether the given <see cref="Guid"/> contains an actual value, i.e. a value that is different from <c>Guid.Empty</c>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/> has a value that is different from <c>Guid.Empty</c>; otherwise, <see langword="false"/>.</returns>
    /// <seealso cref="Guid"/>
    public static bool IsNotEmpty(this Guid input)
        => !input.IsEmpty();

    /// <summary>
    /// Checks whether the given <see cref="Guid"/> is equals to <see langword="null"/> or <c>Guid.Empty</c>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/> is equals to <see langword="null"/> or <c>Guid.Empty</c>; otherwise, <see langword="false"/>.</returns>
    /// <seealso cref="Guid"/>
    public static bool IsEmpty([NotNullWhen(false)] this Guid? input)
        => input.GetValueOrDefault() == Guid.Empty;

    /// <summary>
    /// Checks whether the given <see cref="Guid"/> contains an actual value, i.e. a value that is different from <see langword="null"/> and <c>Guid.Empty</c>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/> has a value that is different from <see langword="null"/> and <c>Guid.Empty</c>; otherwise, <see langword="false"/>.</returns>
    /// <seealso cref="Guid"/>
    public static bool IsNotEmpty([NotNullWhen(true)] this Guid? input)
        => !input.IsEmpty();

    /// <summary>
    /// Checks whether the given <see cref="Guid"/> contains an actual value, i.e. a value that is different from <c>Guid.Empty</c>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/> has a value; otherwise, <see langword="false"/>.</returns>
    /// <seealso cref="Guid"/>
    public static bool HasValue(this Guid input)
        => !input.IsEmpty();

    /// <summary>
    /// Checks whether the given <see cref="Guid"/> contains an actual value, i.e. a value that is different from <see langword="null"/> and <c>Guid.Empty</c>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/> has a value that is different from <see langword="null"/> and <c>Guid.Empty</c>; otherwise, <see langword="false"/>.</returns>
    /// <seealso cref="Guid"/>
    public static bool HasValue([NotNullWhen(true)] this Guid? input)
        => !input.IsEmpty();

    /// <summary>
    /// Gets the actual value of this <see cref="Guid"/> instance, if it is different from <c>Guid.Empty</c>; otherwise, creates a new <see cref="Guid"/> using <see cref="Guid.NewGuid()"/>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <returns>The actual value of this <see cref="Guid"/> instance, if it is different from <c>Guid.Empty</c>; otherwise, a new <see cref="Guid"/> created with <see cref="Guid.NewGuid()"/>.</returns>
    public static Guid GetValueOrCreateNew(this Guid input)
        => input.IsEmpty() ? Guid.NewGuid() : input;

    /// <summary>
    /// Gets the actual value of this <see cref="Guid"/> instance, if it is different from <c>Guid.Empty</c>; otherwise, returns the specified default value.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <param name="defaultValue">The default <see cref="Guid"/> to return if the input is <c>Guid.Empty</c>.</param>
    /// <returns>The actual value of this <see cref="Guid"/> instance, if it is different from <c>Guid.Empty</c>; otherwise, the specified default value.</returns>
    public static Guid GetValueOrDefault(this Guid input, Guid defaultValue)
        => input.IsEmpty() ? defaultValue : input;

    /// <summary>
    /// Gets the actual value of this <see cref="Guid"/> instance, if it is different from <see langword="null"/> and <c>Guid.Empty</c>; otherwise, creates a new <see cref="Guid"/> using <see cref="Guid.NewGuid()"/>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <returns>The actual value of this <see cref="Guid"/> instance, if it is different from <see langword="null"/> and <c>Guid.Empty</c>; otherwise, a new <see cref="Guid"/> created with <see cref="Guid.NewGuid()"/>.</returns>
    public static Guid GetValueOrCreateNew(this Guid? input)
        => input.IsEmpty() ? Guid.NewGuid() : input!.Value;

    /// <summary>
    /// Gets the actual value of this <see cref="Guid"/> instance, if it is different from <see langword="null"/> and <c>Guid.Empty</c>; otherwise, returns the specified default value.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <param name="defaultValue">The default <see cref="Guid"/> to return if the input is <see langword="null"/> or <c>Guid.Empty</c>.</param>
    /// <returns>The actual value of this <see cref="Guid"/> instance, if it is different from <see langword="null"/> and <c>Guid.Empty</c>; otherwise, the specified default value.</returns>
    public static Guid GetValueOrDefault(this Guid? input, Guid defaultValue)
        => input.IsEmpty() ? defaultValue : input!.Value;

#if NET9_0_OR_GREATER
    /// <summary>
    /// Gets the actual value of this <see cref="Guid"/> instance, if it is different from <c>Guid.Empty</c>; otherwise, creates a new <see cref="Guid"/> using <see cref="Guid.NewGuid()"/>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <param name="guidVersion">The version of the <see cref="Guid"/> to create if the input is <c>Guid.Empty</c>.</param>
    /// <returns>The actual value of this <see cref="Guid"/> instance, if it is different from <c>Guid.Empty</c>; otherwise, a new <see cref="Guid"/>.</returns>
    public static Guid GetValueOrCreateNew(this Guid input, GuidVersion guidVersion)
        => input.IsEmpty() ? guidVersion switch
        {
            GuidVersion.Version7 => Guid.CreateVersion7(),
            _ => Guid.NewGuid()
        } : input;

    /// <summary>
    /// Gets the actual value of this <see cref="Guid"/> instance, if it is different from <see langword="null"/> and <c>Guid.Empty</c>; otherwise, creates a new <see cref="Guid"/>.
    /// </summary>
    /// <param name="input">The <see cref="Guid"/> to test.</param>
    /// <param name="guidVersion">The version of the <see cref="Guid"/> to create if the input is <see langword="null"/> or <c>Guid.Empty</c>.</param>
    /// <returns>The actual value of this <see cref="Guid"/> instance, if it is different from <see langword="null"/> and <c>Guid.Empty</c>; otherwise, a new <see cref="Guid"/>.</returns>
    public static Guid GetValueOrCreateNew(this Guid? input, GuidVersion guidVersion)
        => input.IsEmpty() ? guidVersion switch
        {
            GuidVersion.Version7 => Guid.CreateVersion7(),
            _ => Guid.NewGuid()
        } : input!.Value;
#endif
}