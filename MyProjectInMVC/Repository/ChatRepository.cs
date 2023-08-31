using MyProjectInMVC.Controllers;
using MyProjectInMVC.Data;
using MyProjectInMVC.Enums;
using MyProjectInMVC.Models;
using MyProjectInMVC.Models.ChatModels;

namespace MyProjectInMVC.Repository;

public class ChatRepository : IChatRepository
{
    private readonly DataContext _context;

    public ChatRepository(DataContext context)
    {
        _context = context;
    }
    public string Time(DateTime dateTime)
    {
        string time;
        
        DateTime timenow = DateTime.Now;
        TimeSpan diference = timenow - dateTime;

        int minutes = (int)diference.TotalMinutes;
        int hours = (int)diference.TotalHours;
        int days = (int)diference.TotalDays;

        if (minutes < 1)
        {
            time = "Agora";
        }
        else if (minutes > 1 && hours < 1)
        {
            time = $"Há {minutes} min";
        }
        else if(hours > 1 && days < 1)
        {
            time = $"Há {hours} horas";
        }
        else if(days > 1)
        {
            time = $"Há {days} dias";
        }
        else
        {
            time = "Bug";
        }

        return time;
    }

    public modelIndex Model()
    {
        List<UserModel> allusers = _context.Users.Where(x => x.Role == AdmEnum.Aluno).ToList();
            
        List<UserListChatModel> userfalse = new List<UserListChatModel>();
        List<UserListChatModel> usertrue = new List<UserListChatModel>();

        foreach (UserModel item in allusers)
        {
            UserListChatModel user = new UserListChatModel(item.Id);
            user.Name = item.Name;
                
            MessageChatModel? message = _context.Chat.FirstOrDefault(x => x.ReceiveUserId == item.Id || x.SenderUserId == item.Id);
            if (message == null)
            {
                user.Message = "";
                user.Time = "";
                user.Status = true;
            }
            else
            {
                user.Message = message.Message;
                user.Time = Time(message.CreatedAt);
                user.Status = message.Status;
            }

            if (user.Status == false)
            {
                userfalse.Add(user);
            }
            else if(user.Status == true)
            {
                usertrue.Add(user);
            }
        }
        modelIndex model = new modelIndex
        {
            UsersTrue = usertrue,
            UsersFalse = userfalse
        };
        return model;
    }
}