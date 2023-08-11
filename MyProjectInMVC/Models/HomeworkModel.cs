using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class HomeworkModel
    {
        public HomeworkModel()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Título em branco")]
        [StringLength(24, ErrorMessage = "O título deve ter no máximo 24 caracteres")]
        public string Title { get; set; }
        [StringLength(255, ErrorMessage = "O título deve ter no máximo 24 caracteres")]
        public string? Instructions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? FileName { get; set; }
        public byte[]? File { get; set; }

    }
}
