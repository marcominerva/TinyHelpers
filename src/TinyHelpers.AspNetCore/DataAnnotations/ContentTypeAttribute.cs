using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.DataAnnotations;

/// <summary>
/// Describes broad media categories that can be mapped to a predefined set of MIME types.
/// </summary>
public enum FileType
{
    /// <summary>
    /// Represents image MIME types for upload scenarios where downstream processing expects visual media.
    /// </summary>
    Image,

    /// <summary>
    /// Represents video MIME types for upload scenarios where downstream processing expects moving-picture media.
    /// </summary>
    Video,

    /// <summary>
    /// Represents audio MIME types for upload scenarios where downstream processing expects sound media.
    /// </summary>
    Audio
}

/// <summary>
/// Validates that an uploaded <see cref="IFormFile" /> uses one of the permitted content types.
/// </summary>
/// <remarks>
/// Use this attribute when the server depends on a known set of MIME types to avoid accepting files that cannot
/// be rendered, transcoded, or safely processed later in the pipeline.
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ContentTypeAttribute : ValidationAttribute
{
    private readonly IEnumerable<string> validContentTypes;

    private static readonly IEnumerable<string> imageContentTypes = ["image/jpeg", "image/png", "image/gif", "image/bmp", "image/cis-cod", "image/ief", "image/pipeg", "image/svg+xml", "image/tiff", "image/x-cmu-raster", "image/x-cmx", "image/x-icon", "image/x-portable-anymap", "image/x-portable-bitmap", "image/x-portable-graymap", "image/x-portable-pixmap", "image/x-rgb", "image/x-xbitmap", "image/x-xpixmap", "image/x-xwindowdump"];
    private static readonly IEnumerable<string> videoContentTypes = ["video/mp4", "video/ogg", "video/quicktime", "video/mpeg", "video/x-la-asf", "video/x-ms-asf", "video/x-msvideo", "video/x-sgi-movie"];
    private static readonly IEnumerable<string> audioContentTypes = ["audio/basic", "audio/mid", "audio/mpeg", "audio/x-wav", "audio/x-mpegurl", "audio/x-pn-realaudio"];

    private const string DefaultErrorMessage = "The {0} field should have one of the following Content-Types: {1}";

    /// <summary>
    /// Creates a new instance that accepts the specified MIME types.
    /// </summary>
    /// <param name="validContentTypes">The accepted content types, such as <c>image/png</c>.</param>
    public ContentTypeAttribute(params string[] validContentTypes)
        : base(DefaultErrorMessage)
    {
        this.validContentTypes = validContentTypes;
    }

    /// <summary>
    /// Creates a new instance using one of the built-in content type groups.
    /// </summary>
    /// <param name="fileType">The predefined file category to accept.</param>
    public ContentTypeAttribute(FileType fileType)
        : base(DefaultErrorMessage)
    {
        validContentTypes = fileType switch
        {
            FileType.Image => imageContentTypes,
            FileType.Video => videoContentTypes,
            FileType.Audio => audioContentTypes,
            _ => throw new InvalidEnumArgumentException()
        };
    }

    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile formFile && !validContentTypes.Contains(formFile.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }

    /// <summary>
    /// Formats the validation error using the configured content types.
    /// </summary>
    /// <param name="name">The validated member name.</param>
    /// <returns>A localized error message that lists the allowed content types.</returns>
    public override string FormatErrorMessage(string name)
        => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(", ", validContentTypes));
}
