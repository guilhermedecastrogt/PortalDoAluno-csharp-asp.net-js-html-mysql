using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Enums;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Models;
using MyProjectInMVC.Repository;

namespace MyProjectInMVC.Controllers
{
    [AdminUserPage]
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
        
        public IActionResult Info(Guid id)
        {
            HomeworkModel model = _homeworkRepository.FindPerId(id);
            CategoryModel category = _categoryRepository.FindPerId(model.CategoryId);

            
            
            HomeworkModelView homework = new HomeworkModelView
            {
                HomeworkModel = model,
                CategoryName = category.Name
            };
            
            return View(homework);
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
        public IActionResult Edit(HomeworkModelView homework, List<CategoryLevelEnum> level, Guid selectedCategoryIds)
        {
            if (selectedCategoryIds == null || level == null || level.Count == 0 || level[0] == null)
            {
                homework.Categories = _context.Category.ToList();
                TempData["ErrorMessage"] = "Selecione uma categoria e um nível";
                TempData["DataFileMessage"] = "*";
                return View(homework);
            }
            
            try
            {
                ModelState.Remove("level");
                ModelState.Remove("selectedCategoryIds");

                if (ModelState.IsValid)
                {
                    if (homework.DataFile != null && homework.DataFile.Length > 0 && homework.DataFile.Length <= 10 * 1024 * 1024)
                    {
                        //Delete old file
                        if (System.IO.File.Exists(homework.HomeworkModel.FilePath))
                        {
                            System.IO.File.Delete(homework.HomeworkModel.FilePath);
                        }
                    
                        string originalFileName = Path.GetFileName(homework.DataFile.FileName); //get name file
                        string extension = Path.GetExtension(originalFileName); //get type file (.pdf) (.txt) ...

                        string nameFile = homework.HomeworkModel.Id.ToString() + extension;

                        homework.HomeworkModel.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data\\Homeworks", nameFile);
                        var stream = new FileStream(homework.HomeworkModel.FilePath, FileMode.Create);
                        homework.DataFile.CopyToAsync(stream);
                    }
                    
                    homework.HomeworkModel.CategoryId = selectedCategoryIds;
                    homework.HomeworkModel.Level = level[0];

                    _homeworkRepository.Edit(homework.HomeworkModel);
                    TempData["SuccessMessage"] = "Tarefa editada com sucesso";
                    return RedirectToAction("Index");
                }
                homework.Categories = _context.Category.ToList();
                return View(homework);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao editar atividade, tente novamente: {ex}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Create(HomeworkModelView homework, List<CategoryLevelEnum> level, Guid selectedCategoryIds)
        {

            if (selectedCategoryIds == null || level == null || level.Count == 0 || level[0] == null)
            {
                homework.Categories = _context.Category.ToList();
                TempData["ErrorMessage"] = "Selecione uma categoria e um nível";
                TempData["DataFileMessage"] = "*";
                return View(homework);
            }

            try
            {
                ModelState.Remove("level");
                ModelState.Remove("selectedCategoryIds");

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
                    homework.HomeworkModel.Level = level[0];

                    _homeworkRepository.Add(homework.HomeworkModel);
                    TempData["SuccessMessage"] = "Tarefa cadastrada com sucesso";
                    return RedirectToAction("Index");
                }
                homework.Categories = _context.Category.ToList();
                TempData["DataFileMessage"] = "*";
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

        public IActionResult Edit(Guid id)
        {
            HomeworkModelView homework = new HomeworkModelView
            {
                Categories = _categoryRepository.CategoryList(),
                HomeworkModel = _homeworkRepository.FindPerId(id),
            };
            return View(homework);
        }

        [HttpPost]
        public IActionResult Download(string filePath)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", Path.GetFileName(filePath));
        }
        
    }
}