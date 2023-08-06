namespace MyProjectInMVC.Helper
{
    public interface IEmail
    {
        bool Send(string email, string subject, string message);
    }
}
