using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Enums;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Models;
using MyProjectInMVC.Repository;

namespace MyProjectInMVC.Controllers
{
    [LoggedUserPage]
    public class HomeworkController : Controller
    {
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly DataContext _context;
        private readonly ICategoryRepository _categoryRepository;
        public HomeworkController(IHomeworkRepository repository, DataContext datacontext, ICategoryRepository categoryRepository)
        {
            _homeworkRepository = repository;
            _context = datacontext;
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            List<HomeworkModel> homeworks = _homeworkRepository.HomeworkList();
            return View(homeworks);
        }
        
        public IActionResult Create()
        {
            HomeworkModelView homework = new HomeworkModelView
            {
                Categories = _categoryRepository.CategoryList()
            };

            return View(homework);
        }

        [HttpPost]
        public IActionResult Create(HomeworkModelView homework, CategoryLevelEnum level, Guid selectedCategoryIds)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (homework.DataFile != null && homework.DataFile.Length > 0 && homework.DataFile.Length <= 10 * 1024 * 1024)
                        {
                            string originalFileName = Path.GetFileName(homework.DataFile.FileName);
                            string extension = Path.GetExtension(originalFileName);

                            string nameFile = homework.HomeworkModel.Id.ToString() + extension;

                            homework.HomeworkModel.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data\\Homeworks", nameFile);
                            var stream = new FileStream(homework.HomeworkModel.FilePath, FileMode.Create);
                            homework.DataFile.CopyToAsync(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = $"Erro ao salvar arquivo: {ex}";
                        return RedirectToAction("Index");
                    }

                    homework.HomeworkModel.CategoryId = selectedCategoryIds;

                    _homeworkRepository.Add(homework.HomeworkModel);
                    TempData["SuccessMessage"] = "Tarefa cadastrada com sucesso";
                    return RedirectToAction("Index");
                }
                homework.Categories = _context.Category.ToList();
                return View(homework);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao cadastrar tarefa, tente novamente: {ex}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _homeworkRepository.Delete(id);
                TempData["SuccessMessage"] = "Usuário apagado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception error)
            {
                TempData["ErrorMessage"] = $"Erro ao apagar usuário: {error.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}