using System.ComponentModel.DataAnnotations;

namespace MedicalProject.Infrastructure.FileUtil
{
    public class MaxFileCountAttribute : ValidationAttribute
    {
        private readonly int _maxFiles;
        public MaxFileCountAttribute(int maxFiles) => _maxFiles = maxFiles;

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is IList<IFormFile> files && files.Count > _maxFiles)
                return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;
        }
    }

    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedFileExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFileCollection files)
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!_extensions.Contains(extension))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
