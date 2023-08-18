using System.ComponentModel.DataAnnotations;
using MyProjectInMVC.Enums;

namespace MyProjectInMVC.Models
{
    public class HomeworkModelView
    {
        public HomeworkModel HomeworkModel { get; set; }
        public IFormFile? DataFile { get; set; }
        public List<CategoryModel>? Categories { get; set; }
        
        public string? CategoryName{ get; set; }
    }
}
