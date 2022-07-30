using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FileSizeAttribute : ValidationAttribute
{
    private readonly int maxFileSizeInBytes;

    public FileSizeAttribute(int maxFileSizeInBytes)
        : base("{0} size cannot be bigger than {1} Bytes")
    {
        this.maxFileSizeInBytes = maxFileSizeInBytes;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile formFile &&
            formFile.Length > maxFileSizeInBytes)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture,
            ErrorMessageString, name, maxFileSizeInBytes);
    }
}
