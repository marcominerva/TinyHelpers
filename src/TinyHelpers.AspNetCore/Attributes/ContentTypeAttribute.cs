using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.Attributes;

public enum FileType
{
    Image,
    Video,
    Audio
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ContentTypeAttribute : ValidationAttribute
{
    private readonly string[] validContentTypes;
    private readonly string[] imageContentTypes = new string[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/cis-cod", "image/ief", "image/pipeg", "image/svg+xml", "image/tiff", "image/x-cmu-raster", "image/x-cmx", "image/x-icon", "image/x-portable-anymap", "image/x-portable-bitmap", "image/x-portable-graymap", "image/x-portable-pixmap", "image/x-rgb", "image/x-xbitmap", "image/x-xpixmap", "image/x-xwindowdump" };
    private readonly string[] videoContentTypes = new string[] { "video/mp4", "video/ogg", "video/quicktime", "video/mpeg", "video/x-la-asf", "video/x-ms-asf", "video/x-msvideo", "video/x-sgi-movie" };
    private readonly string[] audioContentTypes = new string[] { "audio/basic", "audio/mid", "audio/mpeg", "audio/x-wav", "audio/x-mpegurl", "audio/x-pn-realaudio" };

    public ContentTypeAttribute(params string[] validContentTypes)
        : base("The {0} field should have one of the following content-types {1}")
    {
        this.validContentTypes = this.normalizeDatas(validContentTypes);
    }

    private string[] normalizeDatas(string[] validContentTypes)
    {
        return validContentTypes.Select(s => s.ToLowerInvariant()).ToArray();
    }

    public ContentTypeAttribute(FileType FileType)
        : base("The {0} field should have one of the following Content-Types {1}")
    {

        validContentTypes = FileType switch
        {
            FileType.Image => imageContentTypes,
            FileType.Video => videoContentTypes,
            FileType.Audio => audioContentTypes,
            _ => throw new System.ComponentModel.InvalidEnumArgumentException()
        };
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile formFile && !validContentTypes.Contains(formFile.ContentType))
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }

    public override string FormatErrorMessage(string name)
        => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(", ", validContentTypes));
}
