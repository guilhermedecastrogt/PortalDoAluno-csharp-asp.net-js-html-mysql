﻿using Microsoft.AspNetCore.Mvc;
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
            if (categoryid == Guid.Empty) 
            {
                TempData["ErrorMessage"] = "Acesso não permitido";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                UserModel user = _session.FindSession();
                UserCategoryModel confirm = _dataContext.UserCategory.FirstOrDefault(x => x.UserId == user.Id && x.CategoryId == categoryid);
                if (confirm == null)
                {
                    RedirectToAction("Index", "Home");
                }

                CategoryModel category = _dataContext.Category.FirstOrDefault(x => x.Id == categoryid);



                UniqueCategoryModel model = new UniqueCategoryModel
                {
                    Category = category
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao acessar página, tente novamente {ex}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}