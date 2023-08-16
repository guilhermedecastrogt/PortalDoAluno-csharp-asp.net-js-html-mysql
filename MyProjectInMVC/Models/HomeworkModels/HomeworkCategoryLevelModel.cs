using MyProjectInMVC.Enums;

namespace MyProjectInMVC.Models;

public class HomeworkCategoryLevelModel
{
    public Guid CategoryId { get; set; }
    public CategoryLevelEnum Level { get; set; }
}