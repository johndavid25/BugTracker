using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace BugTracker.Extensions
{
    public class AllowExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage(extension));
                }
            }
            return ValidationResult.Success;
        }
        public string GetErrorMessage(string ext)
        {
            return $"The file extension {ext} is not allowed!";
        }
    }
}
