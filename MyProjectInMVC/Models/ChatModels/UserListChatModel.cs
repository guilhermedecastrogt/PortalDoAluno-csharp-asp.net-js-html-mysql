namespace MyProjectInMVC.Models.ChatModels;

public class UserListChatModel
{
    public UserListChatModel(Guid userId)
    {
        UserId = userId;
    }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Time { get; set; }
    public string Message { get; set; }
    public bool Status { get; set; }
    
}