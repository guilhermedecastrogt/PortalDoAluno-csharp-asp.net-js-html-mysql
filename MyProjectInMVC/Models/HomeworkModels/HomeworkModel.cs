using System.ComponentModel.DataAnnotations;
using MyProjectInMVC.Enums;

namespace MyProjectInMVC.Models
{
    public class HomeworkModel
    {
        public HomeworkModel()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Titulo em branco")]
        [StringLength(255, ErrorMessage = "Tamano excede 24 letras")]
        public string Title { get; set; }
        [StringLength(1255, ErrorMessage = "Tamano excede 255 letras")]
        public string? Instructions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? FilePath { get; set; }
        public Guid? CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
        public CategoryLevelEnum? Level { get; set; }

    }
}
