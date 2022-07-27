using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TinyHelpers.AspNetCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FileSizeAttribute : ValidationAttribute
{
    private readonly int maxFileSizeInMB;

    public FileSizeAttribute(int maxFileSizeInMB)
    {
        this.maxFileSizeInMB = maxFileSizeInMB;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        IFormFile? formFile = value as IFormFile;

        if (formFile == null)
        {
            return ValidationResult.Success;
        }

        if (formFile.Length > maxFileSizeInMB * 1024 * 1024)
        {
            return new ValidationResult($"File size cannot be bigger than {maxFileSizeInMB} MB");
        }

        return ValidationResult.Success;
    }
}
