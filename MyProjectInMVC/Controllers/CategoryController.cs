using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using MyProjectInMVC.Repository;

namespace MyProjectInMVC.Controllers
{
    [LoggedUserPage]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISessao _session;
        public CategoryController(ICategoryRepository categoryrepository, ISessao session)
        {
            _categoryRepository = categoryrepository;
            _session = session;
        }
        public IActionResult Index()
        {
            List<CategoryModel> categoryList = _categoryRepository.CategoryList();
            return View(categoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult ConfirmDelete(Guid id)
        {
            CategoryModel category = _categoryRepository.FindPerId(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(CategoryModel category)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _categoryRepository.Add(category);
                    TempData["SuccessMessage"] = "Categoria cadastrada com successo";
                    return RedirectToAction("Index");
                }
                return View(category);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao cadastrar cateogory, tente novamente: {ex}";
                return RedirectToAction("Index");
            }
            
        }
        public IActionResult Delete(Guid id)
        {
            try
            {
                bool delete = _categoryRepository.Delete(id);
                if (delete)
                {
                    TempData["SuccessMessage"] = "Categoria deletada com sucesso";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = $"Erro ao deletar categoria, tente novamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao deletar categoria, tente novamente: {ex}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Edit(Guid id)
        {
            CategoryModel category = _categoryRepository.FindPerId(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(CategoryModel category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _categoryRepository.Edit(category);

                    TempData["SuccessMessage"] = "Categoria editada com sucesso";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "Erro ao atualizar categoria, tente novamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao atualizar categoria, tente novamente mais tarde: {ex}";
                return RedirectToAction("Index");
            }
        }
    }
}