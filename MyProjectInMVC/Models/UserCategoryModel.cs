using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class UserCategoryModel
    {
        public UserCategoryModel()
        {
            UserCategoryId = Guid.NewGuid();
        }

        [Key]
        public Guid UserCategoryId { get; set; }

        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
