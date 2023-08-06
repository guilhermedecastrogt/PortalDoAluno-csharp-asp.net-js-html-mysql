using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Models;
using MyProjectInMVC.Repository;
using MyProjectInMVC.Helper;

namespace MyProjectInMVC.Controllers
{
    public class ResetPassword : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessao _session;
        public ResetPassword(IUserRepository userrepository, ISessao session)
        {
            _userRepository = userrepository;
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Reset(ResetCurrentPasswordModel resetcurrentpasswordmodel)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    UserModel loggedUser = _session.FindSession();
                    resetcurrentpasswordmodel.Id = loggedUser.Id;

                    _userRepository.ResetPassword(resetcurrentpasswordmodel);
                    TempData["SuccessMessage"] = "Senha alterada com sucesso";

                    return View("Index", resetcurrentpasswordmodel);
                }
                return View("Index", resetcurrentpasswordmodel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao alterar senha: {ex}";
                return View("Index", resetcurrentpasswordmodel);
            }
        }
    }
}
