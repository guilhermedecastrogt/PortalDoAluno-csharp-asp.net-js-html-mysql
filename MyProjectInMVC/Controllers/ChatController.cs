using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using MyProjectInMVC.Data;
using MyProjectInMVC.Enums;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using MyProjectInMVC.Models.ChatModels;
using MyProjectInMVC.Repository;
using NuGet.Protocol.Plugins;

namespace MyProjectInMVC.Controllers
{
    [LoggedUserPage]
    public class ChatController : Controller
    {
        private readonly DataContext _context;
        private readonly IChatRepository _chatRepository;
        private readonly ISessao _session;
        public ChatController(DataContext context, IChatRepository chatRepository, ISessao session)
        {
            _context = context;
            _chatRepository = chatRepository;
            _session = session;
        }
        [AdminUserPage]
        public IActionResult Index()
        {
            modelIndex model = _chatRepository.Model();
            return View(model);
        }

        public IActionResult StudentChat()
        {
            UserModel user = _session.FindSession();

            List<MessageChatModel> messages =
                _context.Chat.Where(x => x.SenderUserId == user.Id || x.ReceiveUserId == user.Id)
                    .OrderBy(x => x.CreatedAt)
                        .ToList();
            
            StudentChatView model = new StudentChatView
            {
                UserSession = user,
                Messages = messages
            };
            return View(model);
        }
        [AdminUserPage]
        public IActionResult IndexChat(Guid id)
        {
            UserModel userSession = _session.FindSession();
            modelIndex userList = _chatRepository.Model();

            List<MessageChatModel> messages =
                _context.Chat.Where(x => x.SenderUserId == id || x.ReceiveUserId == id)
                    .OrderBy(x => x.CreatedAt)
                        .ToList();
            
            modelIndexChat model = new modelIndexChat
            {
                UserList = userList,
                Messages = messages,
                UserSession = userSession.Id
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult InviteMessage(string message, Guid? receiveUserId)
        {
            UserModel userSession = _session.FindSession();
            try
            {
                MessageChatModel msg = _chatRepository.InviteMessage(message);
                if (userSession.Role == AdmEnum.Admin)
                {
                    msg.ReceiveUserId = receiveUserId;
                }

                _context.Chat.Add(msg);
                _context.SaveChanges();
                return RedirectToAction("StudentChat");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao enviar mensagem, tente novamente mais tarde! erro: {ex}";
                if (userSession.Role == AdmEnum.Admin)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("StudentChat");
            }
        }
    }

    public class modelIndex
    {
        public List<UserListChatModel> UsersFalse { get; set; }
        public List<UserListChatModel> UsersTrue { get; set; }
    }
    public class StudentChatView
    {
        public List<MessageChatModel> Messages { get; set; }
        public UserModel UserSession { get; set; }
    }

    public class modelIndexChat
    {
        public modelIndex UserList { get; set; }
        public List<MessageChatModel> Messages { get; set; }
        public Guid UserSession { get; set; }
    }
}