using MyProjectInMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models
{
    public class UserModelNoPassword
    {
        public UserModelNoPassword()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Digite o nome!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Digite o email!")]
        [EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; }
        public AdmEnum Role { get; set; }
        [Required(ErrorMessage = "Digite o telefone!")]
        public string phoneNumber { get; set; }
        [Required(ErrorMessage = "Digite o cpf!")]
        public string Cpf { get; set; }
    }
}
