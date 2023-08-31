using System.ComponentModel.DataAnnotations;

namespace MyProjectInMVC.Models.ChatModels;

public class MessageChatModel
{
    public MessageChatModel()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
    
    [Key]
    public Guid Id { get; set; }
    public Guid SenderUserId { get; set; }
    public Guid? ReceiveUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Status { get; set; }
    public string NameSenderUser { get; set; }
    [Required(ErrorMessage = "Mensagem vazia")]
    [StringLength(600, ErrorMessage = "Tamano excede 600 letras")]
    public string Message { get; set; }
}