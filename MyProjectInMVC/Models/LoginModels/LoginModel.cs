using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email em branco!")]
        [EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha em branco!")]
        public string Password { get; set; }
    }
}
