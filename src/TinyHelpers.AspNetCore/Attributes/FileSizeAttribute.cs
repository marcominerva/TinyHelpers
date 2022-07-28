using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FileSizeAttribute : ValidationAttribute
{
    private readonly int maxFileSizeInBytes;

    public FileSizeAttribute(int maxFileSizeInBytes)
    {
        this.maxFileSizeInBytes = maxFileSizeInBytes;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile formFile)
        {
            if (formFile.Length > maxFileSizeInBytes)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }

        return ValidationResult.Success;
    }

    public override string FormatErrorMessage(string name = "File")
    {
        return $"{name} size cannot be bigger than {maxFileSizeInBytes} Bytes";
    }
}
