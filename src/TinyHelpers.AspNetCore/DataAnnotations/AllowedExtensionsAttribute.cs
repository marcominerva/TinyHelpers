using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class AllowedExtensionsAttribute(params string[] extensions) : ValidationAttribute("Only files with the following extensions are supported: {0}")
{
    private readonly IEnumerable<string> extensions = extensions.Select(e => e.ToLowerInvariant().Replace("*.", string.Empty));

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower()[1..];
            if (!extensions.Contains(extension))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }

        return ValidationResult.Success;
    }

    public override string FormatErrorMessage(string name)
        => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, string.Join(", ", extensions));
}
