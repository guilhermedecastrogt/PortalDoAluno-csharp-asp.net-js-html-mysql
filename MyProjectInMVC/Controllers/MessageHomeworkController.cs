using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using MyProjectInMVC.Data;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Models.MessageModels;

namespace MyProjectInMVC.Controllers
{
    [AdminUserPage]
    public class MessageHomeworkController : Controller
    {
        private readonly DataContext _context;
        public MessageHomeworkController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<MessageHomeworkModel> model = _context.MessageHomework.Where(x => x.Status == false).ToList();
            return View(model);
        }

        public IActionResult ToRespond()
        {
            return View();
        }
    }
}

