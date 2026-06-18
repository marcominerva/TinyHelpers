using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.DataAnnotations;

/// <summary>
/// Validates that an uploaded <see cref="IFormFile" /> uses one of the allowed file extensions.
/// </summary>
/// <param name="extensions">The allowed extensions, with or without the <c>*.</c> prefix.</param>
/// <remarks>
/// This attribute is useful when the extension is part of the contract, such as restricting uploads to image
/// formats that downstream processing or security policies can safely handle.
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class AllowedExtensionsAttribute(params string[] extensions) : ValidationAttribute("Only files with the following extensions are supported: {0}")
{
    private readonly IEnumerable<string> extensions = extensions.Select(e => e.Replace("*.", string.Empty));

    /// <inheritdoc />
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName)[1..];
            if (!extensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }

        return ValidationResult.Success;
    }

    /// <summary>
    /// Formats the validation error using the configured extension list.
    /// </summary>
    /// <param name="name">The validated member name.</param>
    /// <returns>A localized error message that lists the allowed extensions.</returns>
    public override string FormatErrorMessage(string name)
        => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, string.Join(", ", extensions));
}
