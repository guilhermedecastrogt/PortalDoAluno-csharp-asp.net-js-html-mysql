using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Controllers
{
    [LoggedUserPage]
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
            if (categoryid == Guid.Empty) 
            {
                TempData["ErrorMessage"] = "Acesso não permitido";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                UserModel user = _session.FindSession();
                UserCategoryModel userCategory = _dataContext.UserCategory.FirstOrDefault(x => x.UserId == user.Id && x.CategoryId == categoryid);
                if (userCategory == null)
                {
                    RedirectToAction("Index", "Home");
                }

                CategoryModel category = _dataContext.Category.FirstOrDefault(x => x.Id == categoryid);
                
                List<HomeworkModel>? Homeworks = _dataContext.Homeworks.Where(
                    x => x.CategoryId == category.Id && x.Level == userCategory.Level).ToList();

                UniqueCategoryModel model = new UniqueCategoryModel
                {
                    Category = category,
                    Homeworks = Homeworks
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao acessar página, tente novamente {ex}";
                return RedirectToAction("Index", "Home");
            }
        }
        
        
        public IActionResult Details(Guid homeworkId, Guid categoryId)
        {
            HomeworkModel homework = _dataContext.Homeworks.FirstOrDefault(x => x.Id == homeworkId);
            return View(homework);
        }
    }
}
