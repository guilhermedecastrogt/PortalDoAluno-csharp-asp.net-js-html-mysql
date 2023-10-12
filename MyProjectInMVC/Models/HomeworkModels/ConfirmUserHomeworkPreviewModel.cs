using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models;

public class ConfirmUserHomeworkPreviewModel
{
    [Key]
    public Guid HomeworkId { get; set; }

    public Guid UserId { get; set; }
}