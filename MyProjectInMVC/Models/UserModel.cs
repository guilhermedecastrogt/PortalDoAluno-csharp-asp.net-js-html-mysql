using MyProjectInMVC.Enums;
using MyProjectInMVC.Helper;
using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Digite o nome!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Digite o email!")]
        [EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required(ErrorMessage = "Digite a senha!")]
        public string Password { get; set; }
        public AdmEnum Role { get; set; }
        [Required(ErrorMessage = "Digite o telefone!")]
        public string phoneNumber { get; set; }
        [Required(ErrorMessage = "Digite o cpf!")]
        public string Cpf { get; set; }

        public void SetPasswordHash()
        {
            Password = Password.HashGenerate();
        }

        public string SetNewPassword()
        {
            string newPassword = Guid.NewGuid().ToString().Substring(0, 8);
            Password = newPassword.HashGenerate();
            return newPassword;
        }
        public void SetNewPassword(string newPassword)
        {
            Password = newPassword.HashGenerate();
        }
    }
}
