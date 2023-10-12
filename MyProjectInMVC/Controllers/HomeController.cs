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
            return View(categories);
        }
    }
}