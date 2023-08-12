using MyProjectInMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class IndexViewUserEditModel
    {
        public UserModelNoPassword? UserModel { get; set; }
        public List<CategoryModel>? CategoryList { get; set; }
    }
}
