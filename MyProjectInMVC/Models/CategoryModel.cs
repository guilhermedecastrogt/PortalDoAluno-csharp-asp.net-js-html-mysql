using MyProjectInMVC.Enums;

namespace MyProjectInMVC.Models
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}