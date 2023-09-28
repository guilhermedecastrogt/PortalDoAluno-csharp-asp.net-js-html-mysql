namespace MyProjectInMVC.Helper;

public interface IFtpUploader
{
    string UploadFile(FtpConnection model);
    bool DeleteFile(FtpConnection model);
}