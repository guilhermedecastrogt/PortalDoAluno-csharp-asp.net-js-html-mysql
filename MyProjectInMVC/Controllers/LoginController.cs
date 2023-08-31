using Microsoft.AspNetCore.Mvc;
using MyProjectInMVC.Data;
using MyProjectInMVC.Enums;
using MyProjectInMVC.Models;
using MyProjectInMVC.Helper;
using NuGet.Protocol.Plugins;
using MyProjectInMVC.Repository;

namespace MyProjectInMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ISessao _session;
        private readonly IUserRepository _userRepository;
        private readonly IEmail _email;
        public LoginController(DataContext bancocontext, ISessao session, IUserRepository userRepository, IEmail email)
        {
            _session = session;
            _dataContext = bancocontext;
            _userRepository = userRepository;
            _email = email;
        }
        

        public IActionResult Index()
        {
            /*UserModel newuser = new UserModel
            {
                Name = "Guilherme",
                Email = "guilhermecastro1000@gmail.com",
                Password = "123321qwa",
                Role = AdmEnum.Admin,
                phoneNumber = "62 99614 1781",
                Cpf = "076.992.091-84"
            };

            _dataContext.Users.Add(newuser);
            _dataContext.SaveChanges();*/
            
            //If user == Logged
            var user = _session.IfLogged();
            if(user == true)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult Exit()
        {
            _session.RemoveUserSession();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel user = _dataContext.Users.FirstOrDefault
                    (
                        x => x.Email.ToUpper() == login.Email.ToUpper() 
                        && 
                        x.Password == login.Password.HashGenerate()
                    );

                    if (user != null) {
                        
                        _session.CreateUserSession(user);
                        return RedirectToAction("Index", "Home");
                    }
                    TempData["ErrorMessage"] = "Login ou senha incorretos.";
                    return View("Index");
                }
                return View("Index");
            }
            catch (Exception err)
            {
                TempData["ErrorMessage"] = $"Erro ao efetuar Login {err.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost] 
        public IActionResult SendLinkToResetPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     UserModel user  = _userRepository.FindPerEmail(resetPassword.Email);

                    if (user != null)
                    {
                        string newPassword = user.SetNewPassword();

                        string message = $"Sua nova senha é: {newPassword}";

                        bool email = _email.Send(user.Email, "Escola de música Matheus Andreozzi - Redefinir Senha", message);
                        
                        if (email)
                        {
                            _userRepository.Edit(user);
                            TempData["SuccessMessage"] = "Enviamos para o seu email uma nova senha.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Não conseguimos enviar o email, tente novamente.";
                        }

                        return RedirectToAction("Index", "Login");
                    }
                    TempData["ErrorMessage"] = "Email não encontrado no banco de dados.";
                }
                return View("Index");
            }
            catch (Exception err)
            {
                TempData["ErrorMessage"] = $"Erro ao redefinir senha, tente novamente {err.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
