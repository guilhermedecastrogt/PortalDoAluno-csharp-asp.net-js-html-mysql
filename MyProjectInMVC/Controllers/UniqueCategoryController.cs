using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using MyProjectInMVC.Repository;

namespace MyProjectInMVC.Controllers
{
    [LoggedUserPage]
    public class UniqueCategoryController : Controller
    {
        private readonly ISessao _session;
        private readonly DataContext _dataContext;
        private readonly IHomeworkUserRepository _homeworkUserRepository;
        private readonly ICategoryRepository _categoryRepository;
        public UniqueCategoryController(ISessao session, DataContext dataContext, IHomeworkUserRepository homeworkUserRepository, ICategoryRepository categoryRepository)
        {
            _session = session;
            _dataContext = dataContext;
            _homeworkUserRepository = homeworkUserRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(string id)
        {
            try
            {
                UserModel user = _session.FindSession();
                CategoryModel Category = _dataContext.Category.FirstOrDefault(x => x.Slug == id);
                UserCategoryModel userCategory = _dataContext.UserCategory.FirstOrDefault(x => x.UserId == user.Id && x.CategoryId == Category.Id);
                if (userCategory == null)
                {
                    TempData["ErrorMessage"] = "Você não tem permissão para acessar essa página.";
                    return RedirectToAction("Index", "Home");
                }

                CategoryModel category = _dataContext.Category.FirstOrDefault(x => x.Id == Category.Id);
                
                List<HomeworkModel>? Homeworks = _dataContext.Homeworks.Where(
                    x => x.CategoryId == category.Id && x.Level == userCategory.Level).ToList();

                List<HomeworkModel> HomeworksCheck = _homeworkUserRepository.CheckDeleteTrue(Homeworks, user.Id);
                
                Homeworks = HomeworksCheck;

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
            CategoryModel category = _categoryRepository.FindPerId(categoryId);
            ViewBag.Slug = category.Slug;
            return View(homework);
        }

        public IActionResult CompletedHomework(string id)
        {
            try
            {
                CategoryModel Category = _categoryRepository.FindPerSlug(id);
                UserModel user = _session.FindSession();
                UserCategoryModel userCategory = _dataContext.UserCategory.FirstOrDefault(x => x.UserId == user.Id && x.CategoryId == Category.Id);
                if (userCategory == null)
                {
                    TempData["ErrorMessage"] = "Você não tem permissão para acessar essa página.";
                    return RedirectToAction("Index", "Home");
                }
                
                List<HomeworkModel>? Homeworks = _dataContext.Homeworks.Where(
                    x => x.CategoryId == Category.Id && x.Level == userCategory.Level).ToList();

                List<HomeworkModel> HomeworksCheck = _homeworkUserRepository.CheckDeleteFalse(Homeworks, user.Id);
                
                Homeworks = HomeworksCheck;

                UniqueCategoryModel model = new UniqueCategoryModel
                {
                    Category = Category,
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
        public IActionResult TaskTrue(Guid homeworkId, Guid categoryId)
        {
            try
            {
                UserModel user = _session.FindSession();
                HomeworkUserModel model = new HomeworkUserModel();
                model.HomeworkId = homeworkId;
                model.UserId = user.Id;
                model.Status = true;
                _homeworkUserRepository.Create(model);

                TempData["SuccessMessage"] = "Parabéns, você conseguiu concluir a atividade!";
                Guid CategoryId = categoryId;
                return RedirectToAction("Index", "UniqueCategory", new {categoryid = categoryId});
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro interno: {ex}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
