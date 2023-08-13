using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectInMVC.Data;
using MyProjectInMVC.Enums;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using MyProjectInMVC.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyProjectInMVC.Controllers
{
    [AdminUserPage]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly DataContext _context;
        private readonly ICategoryRepository _categoryRepository;
        public UserController(IUserRepository userRepository, DataContext context, ICategoryRepository categoryRepository, ISessao session)
        {
            _userRepository = userRepository;
            _context = context;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            List<UserModel> users = _userRepository.ContactsList();
            return View(users);
        }

        public IActionResult Info(Guid id)
        {
            UserModel user = _userRepository.ListPerId(id);
            return View(user);
        }

        public IActionResult Create()
        {
            var model = new IndexViewUserCreateModel
            {
                Categories = _categoryRepository.CategoryList()
                //User = new UserModel()
            };

            return View(model);
        }

        public IActionResult Edit(Guid id)
        {
            try
            {
                UserModel user = _userRepository.ListPerId(id);
                List<CategoryModel> categories = _categoryRepository.CategoryList();

                UserModelNoPassword usern = new UserModelNoPassword
                {
                    Name = user.Name,
                    Id = user.Id,
                    Cpf = user.Cpf,
                    phoneNumber = user.phoneNumber,
                    Email = user.Email,
                    Role = user.Role
                };

                var viewModel = new IndexViewUserEditModel
                {
                    UserModel = usern,
                    CategoryList = categories
                };

                return View(viewModel);
            }
            catch (Exception error)
            {
                TempData["ErrorMessage"] = $"Houve um erro ao carregar o usuário: {error.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ConfirmDelete(Guid id)
        {
            UserModel user = _userRepository.ListPerId(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult Create(UserModel user, List<Guid> selectedCategoryIds, List<CategoryLevelEnum> level)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    UserModel successCreateUser = _userRepository.Add(user);

                    if (successCreateUser == null)
                    {
                        TempData["ErrorMessage"] = $"Houve um erro ao cadastrar usuário.";
                        return RedirectToAction("Index");
                    }

                    bool successSaveCategory = _userRepository.UserCategoryAdd(selectedCategoryIds, user.Id, level);

                    if(!successSaveCategory)
                    {
                        TempData["ErrorMessage"] = $"Houve um erro ao cadastrar categoria para o usuário.";
                        return RedirectToAction("Index");
                    }

                    TempData["SuccessMessage"] = "Usuário registrado com sucesso!";
                    return RedirectToAction("Index");

                }

                var viewModel = new IndexViewUserCreateModel
                {
                    User = user,
                    Categories = _categoryRepository.CategoryList()
                };

                return View(viewModel);

            }
            catch (Exception error)
            {
                TempData["ErrorMessage"] = $"Houve um erro ao cadastrar usuário: {error.Message}";
                return RedirectToAction("Index");
            }
        }


        public IActionResult Delete(Guid id)
        {
            try
            {
                _userRepository.Delete(id);
                TempData["SuccessMessage"] = "Usuário apagado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception error)
            {
                TempData["ErrorMessage"] = $"Erro ao apagar usuário: {error.Message}";
                return RedirectToAction("Index");
            }
        }
        
        [HttpPost]
        public IActionResult Edit(IndexViewUserEditModel viewModel, List<Guid> selectedCategoryIds, List<CategoryLevelEnum> level)
        {
            if (ModelState.IsValid)
            {
                UserModel user = new UserModel
                {
                    Id = viewModel.UserModel.Id,
                    Name = viewModel.UserModel.Name,
                    Email = viewModel.UserModel.Email,
                    Cpf = viewModel.UserModel.Cpf,
                    phoneNumber = viewModel.UserModel.phoneNumber,
                    Role = viewModel.UserModel.Role
                };

                _userRepository.Edit(user);

                bool successSaveCategory = _userRepository.UserCategoryAdd(selectedCategoryIds, viewModel.UserModel.Id, level);

                if(!successSaveCategory)
                {
                    TempData["ErrorMessage"] = "Usuário erro ao atualizar as tategorias do usuário!";
                    return RedirectToAction("Index");
                }

                TempData["SuccessMessage"] = "Usuário editado com sucesso!";
                return RedirectToAction("Index");
            }

            viewModel.CategoryList = _categoryRepository.CategoryList();
            return View(viewModel);
        }

        public IActionResult SelectCategories(Guid userid)
        { 
            List<CategoryModel> categories = _categoryRepository.CategoryList();
            ViewBag.UserId = userid;
            return View(categories);
        }

        [HttpPost]
        public ActionResult SelectCategories(Guid userid, List<Guid> selectedCategoryIds)
        {
            try
            {
                _context.UserCategory.RemoveRange(_context.UserCategory.Where(x => x.UserId == userid));

                foreach (var categoryId in selectedCategoryIds)
                {
                    _context.UserCategory.Add(new UserCategoryModel { UserId = userid, CategoryId = categoryId});
                }

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Categorias adicionadas ao usuário com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Falha ao adicionar categorias ao usuário, tente novamente: {ex}";
                return RedirectToAction("Index");
            }
        }
    }
}
