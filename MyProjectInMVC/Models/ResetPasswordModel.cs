using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email em branco!")]
        [EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; }
    }
}