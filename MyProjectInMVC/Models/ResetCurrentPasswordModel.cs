using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class ResetCurrentPasswordModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Digite a sua senha atual!")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage ="Digite sua nova senha!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirme a sua senha")]
        [Compare("NewPassword", ErrorMessage = "Senhas diferentes")]
        public string ConfirmNewPassword { get; set; }

    }
}
