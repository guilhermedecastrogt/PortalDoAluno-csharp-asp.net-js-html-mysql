using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using MyProjectInMVC.Models.MessageModels;
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
            UserModel user = _session.FindSession();
            HomeworkModel homework = _dataContext.Homeworks.FirstOrDefault(x => x.Id == homeworkId);
            CategoryModel category = _categoryRepository.FindPerId(categoryId);
            UserCategoryModel acess = _dataContext.UserCategory.FirstOrDefault(x => x.UserId == user.Id && x.CategoryId == categoryId);
            HomeworkUserModel HomeworkUser = _dataContext.HomeworkUserModel.FirstOrDefault(x => x.HomeworkId == homework.Id && x.UserId == user.Id && x.Status == true);
            if (acess == null)
            {
                TempData["ErrorMessage"] = "Você não tem permissão para acessar essa página.";
                return RedirectToAction("Index", "Home");
            }
            
            if (HomeworkUser != null)
            {
                ViewBag.True = HomeworkUser.Status;
            }

            List<MessageHomeworkModel> messages = _dataContext.MessageHomework
                .Where(x => (x.SenderUserId == user.Id && x.HomeworkId == homework.Id) || 
                            (x.ReceiveUserId == user.Id && x.HomeworkId == homework.Id))
                .OrderBy(x => x.CreatedAt)
                .ToList();

            DetailsViewModel model = new DetailsViewModel
            {
                UserSession = user.Id,
                Homework = homework,
                Messages = messages
            };
            
            ViewBag.Slug = category.Slug;
            return View(model);
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
        [HttpPost]
        public IActionResult TaskTrue(Guid homeworkId, Guid categoryId)
        {
            try
            {
                CategoryModel category = _categoryRepository.FindPerId(categoryId);
                UserModel user = _session.FindSession();
                HomeworkUserModel model = new HomeworkUserModel();
                model.HomeworkId = homeworkId;
                model.UserId = user.Id;
                model.Status = true;
                _homeworkUserRepository.Create(model);

                TempData["SuccessMessage"] = "Parabéns, você conseguiu concluir a atividade!";
                return RedirectToAction("Index", "UniqueCategory", new {id = category.Slug});
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro interno: {ex}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult TaskFalse(Guid homeworkId, Guid categoryId)
        {
            try
            {
                CategoryModel category = _categoryRepository.FindPerId(categoryId);
                UserModel user = _session.FindSession();
                HomeworkUserModel model =_dataContext.HomeworkUserModel.FirstOrDefault(x => 
                        x.HomeworkId == homeworkId && 
                        x.UserId == user.Id
                );
                
                _dataContext.Remove(model);
                _dataContext.SaveChanges();
                
                TempData["SuccessMessage"] = "Tarefa disponibilizada para entregar novamente na página 'Concluir'";
                return RedirectToAction("Index", "UniqueCategory", new {id = category.Slug});
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro interno: {ex}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult InviteMessage(string message, Guid homeworkId, Guid categoryId)
        {
            try
            {
                UserModel user = _session.FindSession();

                MessageHomeworkModel? check = _dataContext.MessageHomework.FirstOrDefault(x =>
                    x.HomeworkId == homeworkId && x.SenderUserId == user.Id && x.Status == false);
                MessageHomeworkModel createMessage = new MessageHomeworkModel();
                if (check == null)
                {
                    createMessage.SenderUserId = user.Id;
                    createMessage.ReceiveUserId = null;
                    createMessage.HomeworkId = homeworkId;
                    createMessage.Message = message;
                    createMessage.Status = false;
                    createMessage.NameSenderUser = user.Name;
                }
                else
                {
                    createMessage.SenderUserId = user.Id;
                    createMessage.ReceiveUserId = null;
                    createMessage.HomeworkId = homeworkId;
                    createMessage.Message = message;
                    createMessage.Status = true;
                    createMessage.NameSenderUser = user.Name;
                }
                
                _dataContext.MessageHomework.Add(createMessage);
                _dataContext.SaveChanges();
                TempData["SuccessMessage"] = "Mensagem enviada com successo, aguarde que o professor logo responderá";
                return RedirectToAction("Details", "UniqueCategory",
                    new { homeworkId = homeworkId, categoryId = categoryId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro interno: {ex}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
