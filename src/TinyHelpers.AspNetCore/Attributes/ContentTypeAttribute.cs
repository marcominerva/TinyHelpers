using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    private readonly IEnumerable<string> validContentTypes;

    private static readonly IEnumerable imageContentTypes = new string[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/cis-cod", "image/ief", "image/pipeg", "image/svg+xml", "image/tiff", "image/x-cmu-raster", "image/x-cmx", "image/x-icon", "image/x-portable-anymap", "image/x-portable-bitmap", "image/x-portable-graymap", "image/x-portable-pixmap", "image/x-rgb", "image/x-xbitmap", "image/x-xpixmap", "image/x-xwindowdump" };
    private static readonly IEnumerable videoContentTypes = new string[] { "video/mp4", "video/ogg", "video/quicktime", "video/mpeg", "video/x-la-asf", "video/x-ms-asf", "video/x-msvideo", "video/x-sgi-movie" };
    private static readonly IEnumerable audioContentTypes = new string[] { "audio/basic", "audio/mid", "audio/mpeg", "audio/x-wav", "audio/x-mpegurl", "audio/x-pn-realaudio" };

    private const string DefaultErrorMessage = "The {0} field should have one of the following Content-Types: {1}";

    public ContentTypeAttribute(params string[] validContentTypes)
        : base(DefaultErrorMessage)
    {
        this.validContentTypes = validContentTypes.Select(s => s.ToLowerInvariant());
    }

    public ContentTypeAttribute(FileType fileType)
        : base(DefaultErrorMessage)
    {
        validContentTypes = (IEnumerable<string>) (fileType switch
        {
            FileType.Image => imageContentTypes,
            FileType.Video => videoContentTypes,
            FileType.Audio => audioContentTypes,
            _ => throw new InvalidEnumArgumentException()
        });
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
