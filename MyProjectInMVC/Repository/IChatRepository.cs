using MyProjectInMVC.Controllers;
using MyProjectInMVC.Models;
using MyProjectInMVC.Models.ChatModels;

namespace MyProjectInMVC.Repository;

public interface IChatRepository
{
    string Time(DateTime datetime);
    modelIndex Model();
    MessageChatModel InviteMessage(string message);
}