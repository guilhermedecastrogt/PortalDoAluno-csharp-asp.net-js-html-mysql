using MyProjectInMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class CategoryModel
    {

        public CategoryModel()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Digite o nome da categoria")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Digite o slug da categoria")]
        [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "O slug não pode ter espaços nem caracteres especiais")]
        public string Slug { get; set; }
    }
}