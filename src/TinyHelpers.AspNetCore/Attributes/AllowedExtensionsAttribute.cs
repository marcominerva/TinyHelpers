using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly IEnumerable<string> extensions;

    public AllowedExtensionsAttribute(params string[] extensions)
    {
        this.extensions = extensions.Select(e => e.ToLower().Replace("*.", string.Empty));
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower()[1..];
            if (!extensions.Contains(extension))
            {
                return new ValidationResult(GetErrorMessage());
            }
        }

        return ValidationResult.Success;
    }

    public string GetErrorMessage()
        => $"Only files with the following extensions are supported: {string.Join(", ", extensions)}";
}
