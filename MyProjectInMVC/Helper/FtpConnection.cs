namespace MyProjectInMVC.Helper;

public class FtpConnection
{
    public FtpConnection(IConfiguration _configuration)
    {
        try
        {
            ftpUsername = _configuration.GetValue<string>("FtpConnection:Username");
            ftpPassword = _configuration.GetValue<string>("FtpConnection:Password");
            ftpServerUrl = _configuration.GetValue<string>("FtpConnection:ServerUrl");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro {ex}");
        }
    }
    
    public IFormFile File { get; set; }
    public string? ftpServerUrl { get; set; }
    public string? ftpUsername { get; set; }
    public string? ftpPassword { get; set; }
    public string remoteFileName { get; set; }
    public string path { get; set; }
}