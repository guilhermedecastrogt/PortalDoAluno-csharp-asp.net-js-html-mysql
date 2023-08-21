using MyProjectInMVC.Models.MessageModels;

namespace MyProjectInMVC.Models;

public class DetailsViewModel
{
    public HomeworkModel Homework { get; set; }
    public List<MessageHomeworkModel> Messages { get; set; }
    public Guid UserSession { get; set; }
}