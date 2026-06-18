using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.DataAnnotations;

/// <summary>
/// Validates that an uploaded <see cref="IFormFile" /> does not exceed a configured maximum size.
/// </summary>
/// <param name="maxFileSizeInBytes">The maximum accepted file size in bytes.</param>
/// <remarks>
/// This is intended for request boundaries where rejecting oversized payloads early is cheaper and clearer than
/// allowing the file to progress into later validation or storage stages. The limit also documents the upload
/// contract that clients should honor before sending content.
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class FileSizeAttribute(int maxFileSizeInBytes) : ValidationAttribute("The {0} field size cannot be bigger than {1} bytes")
{
    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile formFile && formFile.Length > maxFileSizeInBytes)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }

    /// <summary>
    /// Formats the validation error with the configured byte limit so rejected uploads report the exact contract boundary.
    /// </summary>
    /// <param name="name">The validated member name.</param>
    /// <returns>A localized error message that includes the maximum file size.</returns>
    public override string FormatErrorMessage(string name)
        => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, maxFileSizeInBytes);
}
