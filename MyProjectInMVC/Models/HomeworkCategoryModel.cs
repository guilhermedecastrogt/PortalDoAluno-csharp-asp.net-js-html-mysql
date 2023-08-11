using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class HomeworkCategoryModel
    {
        public HomeworkCategoryModel()
        {
            HomeworkCategoryId = Guid.NewGuid();
        }
        [Key]
        public Guid HomeworkCategoryId { get; set; }
        public Guid HomeworkId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
