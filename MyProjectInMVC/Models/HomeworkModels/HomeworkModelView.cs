using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class HomeworkModelView
    {
        public HomeworkModel HomeworkModel { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "O slug não pode ter espaços nem caracteres especiais")]
        public string NameFile { get; set; }

        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".pdf", ".doc", ".docm", ".docx", ".xlsx", ".xls"})]
        public IFormFile DataFile { get; set; }
        public List<CategoryModel> Categories { get; set; }

        //MaxFileSize
        public class MaxFileSizeAttribute : ValidationAttribute
        {
            private readonly int _maxFileSize;

            public MaxFileSizeAttribute(int maxFileSize)
            {
                _maxFileSize = maxFileSize;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is IFormFile file)
                {
                    if (file.Length > _maxFileSize)
                    {
                        return new ValidationResult($"O tamanho do arquivo não pode ser maior que {_maxFileSize / 1024 / 1024} MB.");
                    }
                }

                return ValidationResult.Success;
            }
        }
        //AlloweExtension
        public class AllowedExtensionsAttribute : ValidationAttribute
        {
            private readonly string[] _extensions;

            public AllowedExtensionsAttribute(string[] extensions)
            {
                _extensions = extensions;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is IFormFile file)
                {
                    var fileExtension = System.IO.Path.GetExtension(file.FileName);

                    if (!_extensions.Contains(fileExtension.ToLower()))
                    {
                        return new ValidationResult($"As extensões permitidas são: {string.Join(", ", _extensions)}");
                    }
                }

                return ValidationResult.Success;
            }
        }
    }
}
