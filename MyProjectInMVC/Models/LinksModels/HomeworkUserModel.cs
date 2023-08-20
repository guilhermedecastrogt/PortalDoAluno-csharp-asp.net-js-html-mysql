using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models;

public class HomeworkUserModel
{
    [Key]
    public Guid HomeworkId { get; set; }
    public Guid UserId { get; set; }
    public bool Status { get; set; }
}