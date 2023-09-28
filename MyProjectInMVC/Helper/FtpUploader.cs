using System;
using System.IO;
using System.Net;
using MyProjectInMVC.Helper;

public class FtpUploader : IFtpUploader
{
    public string UploadFile(FtpConnection model)
    {
        try
        {
            // Create FTP request
            //model.remoteFileName = Uri.EscapeDataString(model.remoteFileName);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(model.ftpServerUrl + "/" + model.path + "/" +model.remoteFileName));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(model.ftpUsername, model.ftpPassword);
            request.UseBinary = true;

            using (Stream requestStream = request.GetRequestStream())
            {
                // Copy IFormFile content to stream request
                model.File.CopyTo(requestStream);
            }

            // Request for response
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Upload completo. Status: " + response.StatusDescription);

            response.Close();
            string path = "https://arquivos.matheusandreozzi.com.br/portaldoalunomatheus" + "/" + model.path + "/" + model.remoteFileName;
            return path;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao enviar o arquivo: " + ex);
            return null;
        }
    }

    public bool DeleteFile(FtpConnection model)
    {
        try
        {
            // Create request FTP
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(model.ftpServerUrl + "/" + model.path + "/" +model.remoteFileName));
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(model.ftpUsername, model.ftpPassword);

            // Request for response
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Arquivo deletado. Status: " + response.StatusDescription);

            response.Close();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao deletar o arquivo: " + ex.Message);
            return false;
        }
    }
}