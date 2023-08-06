using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Filters;

namespace MyProjectInMVC.Controllers
{
    [LoggedUserPage]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}