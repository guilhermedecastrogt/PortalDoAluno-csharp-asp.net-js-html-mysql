using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Controllers
{
    
    public class UniqueCategoryController : Controller
    {
        private readonly ISessao _session;
        private readonly DataContext _dataContext;
        public UniqueCategoryController(ISessao session, DataContext dataContext)
        {
            _session = session;
            _dataContext = dataContext;

        }

        public IActionResult Index(Guid categoryid)
        {
            UserModel user = _session.FindSession();
            UserCategoryModel confirm = _dataContext.UserCategory.FirstOrDefault
                (
                    x => x.UserId == user.Id
                    &&
                    x.CategoryId == categoryid
                );

            if (confirm == null)
            {
                RedirectToAction("Index", "Home");
            }

            CategoryModel category = _dataContext.Category.FirstOrDefault(x => x.Id == categoryid);

            return View(category);
        }
    }
}
