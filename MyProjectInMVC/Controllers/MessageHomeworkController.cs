using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using MyProjectInMVC.Data;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using MyProjectInMVC.Models.MessageModels;
using MyProjectInMVC.Repository;

namespace MyProjectInMVC.Controllers
{
    [AdminUserPage]
    public class MessageHomeworkController : Controller
    {
        private readonly DataContext _context;
        private readonly ISessao _session;
        private readonly IUserRepository _userRepository;
        private readonly IHomeworkRepository _homeworkRepository;
        public MessageHomeworkController(DataContext context, ISessao session, IUserRepository userRepository, IHomeworkRepository homeworkRepository)
        {
            _context = context;
            _session = session;
            _userRepository = userRepository;
            _homeworkRepository = homeworkRepository;
        }
        public IActionResult Index()
        {
            List<MessageHomeworkModel> model = _context.MessageHomework.Where(x => x.Status == false).ToList();
            return View(model);
        }

        public IActionResult ToRespond(Guid id)
        {
            UserModel user = _session.FindSession();
            MessageHomeworkModel message = _context.MessageHomework.FirstOrDefault(x=> x.Id == id);
            UserModel student = _userRepository.ListPerId(message.SenderUserId);
            HomeworkModel homework = _homeworkRepository.FindPerId(message.HomeworkId);
            List<MessageHomeworkModel> messages = _context.MessageHomework.Where(x =>
                (x.HomeworkId == message.HomeworkId && x.SenderUserId == student.Id) ||
                (x.HomeworkId == message.HomeworkId && x.SenderUserId == user.Id && x.ReceiveUserId == student.Id))
                .OrderBy(x => x.CreatedAt)
                .ToList();

            ToRespondViewModel model = new ToRespondViewModel
            {
                Messages = messages,
                StudentId = student.Id,
                Homework = homework,
                UserSession = user.Id,
                MessageTrueId = message.Id
            };
            
            return View(model);
        }

        [HttpPost]
        public IActionResult ToRespondMessage(string message, Guid homeworkId, Guid categoryId, Guid studentId, Guid messagetrueId)
        {
            try
            {
                UserModel user = _session.FindSession();
                MessageHomeworkModel createMessage = new MessageHomeworkModel
                {
                    SenderUserId = user.Id,
                    ReceiveUserId = studentId,
                    HomeworkId = homeworkId,
                    Message = message,
                    Status = true,
                    NameSenderUser = user.Name
                };
                
                _context.MessageHomework.Add(createMessage);

                MessageHomeworkModel messagetrue = _context.MessageHomework.FirstOrDefault(x => x.Id == messagetrueId);
                messagetrue.Status = true;
                _context.MessageHomework.Update(messagetrue);
                
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "Mensagem respondida com sucesso!";
                return RedirectToAction("ToRespond", "MessageHomework",
                    new { id = messagetrueId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro interno: {ex}";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AllMessages()
        {
            List<MessageHomeworkModel> allMessages = _context.MessageHomework.ToList();
            
            List<HomeworkModel> Homeworks = new List<HomeworkModel>();
            
            int i = 0;
            foreach (MessageHomeworkModel item in allMessages)
            {
                HomeworkModel homework = _context.Homeworks.FirstOrDefault(x => x.Id == item.HomeworkId);
                Homeworks.Add(homework);
                i++;
            }

            AllMessagesViewModel model = new AllMessagesViewModel
            {
                allMessages = allMessages,
                Homework = Homeworks
            };
            
            return View(model);
        }

        public IActionResult Delete(Guid id)
        {
            try
            {
                MessageHomeworkModel message = _context.MessageHomework.FirstOrDefault(x => x.Id == id);
                _context.MessageHomework.Remove(message);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Mensagem apagada com sucesso";
                return RedirectToAction("AllMessages");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao apagar mensagem, tente novamente: {ex}";
                return RedirectToAction("AllMessages");
            }
        }

        public IActionResult Already(Guid id)
        {
            try
            {
                MessageHomeworkModel model = _context.MessageHomework.FirstOrDefault(x => x.Id == id);
                model.Status = true;
                _context.Update(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Mensagem respondida!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro interno, tente novamente: {ex}";
                return RedirectToAction("Index");
                
            }
        }

        public class AllMessagesViewModel
        {
            public List<MessageHomeworkModel> allMessages { get; set; }
            public List<HomeworkModel> Homework { get; set; }
        }
    }
}

