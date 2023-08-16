using MyProjectInMVC.Enums;

namespace MyProjectInMVC.Models
{
    public class IndexViewUserCreateModel
    {
        public UserModel User { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}
