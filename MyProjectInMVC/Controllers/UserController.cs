using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectInMVC.Data;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using MyProjectInMVC.Repository;

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
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            UserModel user = _userRepository.ListPerId(id);
            return View(user);
        }

        public IActionResult ConfirmDelete(Guid id)
        {
            UserModel user = _userRepository.ListPerId(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult Create(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userRepository.Add(user);
                    TempData["SuccessMessage"] = "Usuário registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(user);
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
        public IActionResult Edit(UserModelNoPassword usernopassword)
        {
            try
            {
                UserModel user = null;
                if (ModelState.IsValid)
                {
                    user = new UserModel()
                    {
                        Id = usernopassword.Id,
                        Name = usernopassword.Name,
                        Email = usernopassword.Email,
                        Cpf = usernopassword.Cpf,
                        phoneNumber = usernopassword.phoneNumber,
                        Role = usernopassword.Role
                    };

                    user = _userRepository.Edit(user);
                    TempData["SuccessMessage"] = "Usuário editado co successo!";
                    return RedirectToAction("Index");
                }
                return View(user);
            }
            catch (Exception error)
            {
                TempData["ErrorMessage"] = $"Houve um erro ao cadastrar usuário: {error.Message}";
                return RedirectToAction("Index");
            }

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
