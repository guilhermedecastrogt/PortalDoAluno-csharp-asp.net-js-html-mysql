using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Enums;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
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
        private readonly IFtpUploader _ftpUploader;
        private readonly IConfiguration _configuration;
        public HomeworkController(
            IHomeworkRepository repository,
            DataContext datacontext,
            ICategoryRepository categoryRepository,
            IFtpUploader ftpUploader,
            IConfiguration configuration
        )
        {
            _homeworkRepository = repository;
            _context = datacontext;
            _categoryRepository = categoryRepository;
            _ftpUploader = ftpUploader;
            _configuration = configuration;
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
                        try
                        {
                            // Remove File
                                HomeworkModel? homeworkPath =
                                    _context.Homeworks.FirstOrDefault(x => x.Id == homework.HomeworkModel.Id);
                                string path = Path.GetExtension(homeworkPath.FilePath);

                                FtpConnection model = new FtpConnection(_configuration);
                                model.ftpServerUrl = model.ftpServerUrl + "/homeworks/";
                                model.remoteFileName = homework.HomeworkModel.Id + path;
                                bool check = _ftpUploader.DeleteFile(model);
                                if (!check)
                                {
                                    Console.WriteLine("Erro ao salvar arquivo.");
                                    TempData["ErrorMessage"] = "Erro ao salvar arquivo";
                                    return RedirectToAction("Index");
                                }
                            //
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);  
                        }
                        
                        //Save File
                        string filename = homework.DataFile.FileName;
                        string fileExtension = Path.GetExtension(filename);
                        FtpConnection connection = new FtpConnection(_configuration)
                        {
                            File = homework.DataFile,
                            remoteFileName = homework.HomeworkModel.Id.ToString() + fileExtension,
                            path = "homeworks",
                        };
                        string? pathSave = _ftpUploader.UploadFile(connection);
                        if (pathSave == null)
                        {
                            TempData["ErrorMessage"] = $"Erro ao salvar arquivo no servidor!";
                            return RedirectToAction("Index");
                        }
                        homework.HomeworkModel.FilePath = pathSave;
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
                            string filename = homework.DataFile.FileName;
                            string fileExtension = Path.GetExtension(filename);
                            FtpConnection model = new FtpConnection(_configuration)
                            {
                                File = homework.DataFile,
                                remoteFileName = homework.HomeworkModel.Id.ToString() + fileExtension,
                                path = "homeworks",
                            };
                            string? path = _ftpUploader.UploadFile(model);
                            if (path == null)
                            {
                                TempData["ErrorMessage"] = $"Erro ao salvar arquivo no servidor!";
                                return RedirectToAction("Index");
                            }
                            homework.HomeworkModel.FilePath = path;
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
    }
}