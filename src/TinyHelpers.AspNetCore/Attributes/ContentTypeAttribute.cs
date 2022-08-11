using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ContentTypeAttribute : ValidationAttribute
{
    private readonly string[] validContentType;
    private readonly string[] imageContentTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
    private readonly string[] videoContentTypes = new string[] { "video/mp4", "video/ogg", "video/quicktime" };

    public ContentTypeAttribute(string[] ValidContentType)
    {
        validContentType = ValidContentType;
    }

    public ContentTypeAttribute(ContentTypeGroup contentTypeGroup)
        : base("The {0} field should have one of the following content-types {1}")
    {
        switch (contentTypeGroup)
        {
            case ContentTypeGroup.Image:
                validContentType = imageContentTypes;
                break;
            case ContentTypeGroup.Video:
                validContentType = videoContentTypes;
                break;
            default: throw new System.ComponentModel.InvalidEnumArgumentException();
        }
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile formFile && !validContentType.Contains(formFile.ContentType))
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }

    public override string FormatErrorMessage(string name)
        => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(", ", validContentType));
}

public enum ContentTypeGroup
{
    Image,
    Video
}
