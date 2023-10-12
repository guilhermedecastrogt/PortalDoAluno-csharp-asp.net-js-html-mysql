using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using MyProjectInMVC.Repository;
using System.Diagnostics;

namespace MyProjectInMVC.Controllers
{
    [LoggedUserPage]
    public class HomeController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly DataContext _dataContext;
        private readonly ISessao _session;
        public HomeController(ICategoryRepository category, DataContext dataContext, ISessao session)
        {
            _categoryRepository = category;
            _dataContext = dataContext;
            _session = session;

        }

        public IActionResult Index()
        {
            UserModel user = _session.FindSession();
            List<UserCategoryModel> UserCategoryIds = _dataContext.UserCategory.Where(x => x.UserId == user.Id).ToList();
            List<CategoryModel> allCategories = _categoryRepository.CategoryList();
            List<CategoryModel> categories = new List<CategoryModel>();

            foreach (CategoryModel category in allCategories)
            {
                foreach(UserCategoryModel item in UserCategoryIds)
                {
                    if (category.Id == item.CategoryId)
                    {
                        categories.Add(category);
                    }
                }
            }

            List<CategoryModel> redCategory = new List<CategoryModel>();
            List<CategoryModel> Categories = new List<CategoryModel>();
            
            foreach (CategoryModel item in categories)
            {
                List<HomeworkModel> homeworks = _dataContext.Homeworks.Where(x =>
                    x.CategoryId == item.Id).ToList();
                
                foreach (HomeworkModel homework in homeworks)
                {
                    ConfirmUserHomeworkPreviewModel? check = _dataContext.ConfirmUserHomeworkPreview.FirstOrDefault(
                        x =>
                            x.HomeworkId == homework.Id &&
                            x.UserId == user.Id
                    );
                    if (check == null)
                    {
                        redCategory.Add(item);
                    }
                    else
                    {
                        Categories.Add(item);
                    }
                }
            }

            IndexHomeModel model = new IndexHomeModel()
            {
                Categories = Categories,
                RedCategory = redCategory
            };
            return View(model);
        }
    }

    public class IndexHomeModel
    {
        public List<CategoryModel>? RedCategory { get; set; }
        public List<CategoryModel>? Categories { get; set; }
    }
}