using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Enums;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Models;
using MyProjectInMVC.Models.ChatModels;
using MyProjectInMVC.Repository;
using NuGet.Protocol.Plugins;

namespace MyProjectInMVC.Controllers
{
    [AdminUserPage]
    public class ChatController : Controller
    {
        private readonly DataContext _context;
        private readonly IChatRepository _chatRepository;

        public ChatController(DataContext context, IChatRepository chatRepository)
        {
            _context = context;
            _chatRepository = chatRepository;
        }

        public IActionResult Index()
        {
            modelIndex model = _chatRepository.Model();
            return View(model);
        }
    }

    public class modelIndex
    {
        public List<UserListChatModel> UsersFalse { get; set; }
        public List<UserListChatModel> UsersTrue { get; set; }
    }
    public class aaa
    {
        public List<UserModel> Users { get; set; }
        public List<MessageChatModel> Messages { get; set; }
    }
}