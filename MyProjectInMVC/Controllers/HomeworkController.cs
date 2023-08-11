using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Filters;

namespace MyProjectInMVC.Controllers
{
    public class HomeworkController : Controller
    {
        [LoggedUserPage]
        public IActionResult Index()
        {
            return View();
        }
    }
}
