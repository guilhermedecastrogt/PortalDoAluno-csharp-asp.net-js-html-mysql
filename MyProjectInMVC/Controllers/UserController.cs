using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Filters;
using MyProjectInMVC.Models;
using MyProjectInMVC.Repository;

namespace MyProjectInMVC.Controllers
{
    [AdminUserPage]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            catch(Exception error)
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
            catch(Exception error)
            {
                TempData["ErrorMessage"] = $"Houve um erro ao cadastrar usuário: {error.Message}";
                return RedirectToAction("Index");
            }
            
        }

    }
}
