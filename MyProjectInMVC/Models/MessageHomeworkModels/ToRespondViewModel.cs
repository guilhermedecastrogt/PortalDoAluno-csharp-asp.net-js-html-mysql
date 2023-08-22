namespace MyProjectInMVC.Models.MessageModels
{
    public class ToRespondViewModel
    {
        public List<MessageHomeworkModel> Messages { get; set; }
        public Guid StudentId { get; set; }
        public HomeworkModel Homework { get; set; }
        public Guid UserSession { get; set; }
        public Guid MessageTrueId { get; set; }
    }
}