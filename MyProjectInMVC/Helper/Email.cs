using System.Net;
using System.Net.Mail;

namespace MyProjectInMVC.Helper
{
    public class Email : IEmail
    {
        private readonly IConfiguration _configuration;
        public Email(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Send(string email, string subject, string message)
        {
            try
            {
                string? host, password, username, name, portstring;
                int port;
                try
                {
                    host = Environment.GetEnvironmentVariable("SMTPConnectionHost");
                    password = Environment.GetEnvironmentVariable("SMTPConnectionPassword");
                    username = Environment.GetEnvironmentVariable("SMTPConnectionUsername");
                    portstring = Environment.GetEnvironmentVariable("SMTPConnectionPort");
                    name = Environment.GetEnvironmentVariable("SMTPConnectionName");
                    port = int.Parse(portstring);
                }
                catch
                {
                    host = _configuration.GetValue<string>("SMTP:Host");
                    password = _configuration.GetValue<string>("SMTP:Password");
                    username = _configuration.GetValue<string>("SMTP:UserName");
                    port = _configuration.GetValue<int>("SMTP:Port");
                    name = _configuration.GetValue<string>("SMTP:Name");
                }
                

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(username, name)
                };

                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials = new NetworkCredential(username, password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    return true;
                }
            }
            catch(System.Exception ex)
            {
                return false;
            }
        }
    }
}
