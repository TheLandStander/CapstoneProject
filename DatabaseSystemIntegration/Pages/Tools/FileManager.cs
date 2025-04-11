using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

public static class FileManager
{
    private static readonly string _ftp = "waws-prod-yt1-073.ftp.azurewebsites.windows.net/site/wwwroot/wwwroot/fileupload";
    private static readonly string _ftpUsername = "CARE-JMU2025\\$CARE-JMU2025";
    private static readonly string _ftpPassword = "CTPlJjGt1FT2dThLHsorcdcj6hCvFDJm1uP4yKxRgScERPTWPZhT6k6Ydbsb";

    public static async Task<bool> UploadFileAsync(string id, Stream fileStream)
    {
        try
        {
            string uri = $"ftp://{_ftp}/{id}";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);
            request.UseBinary = true;
            request.UsePassive = true; 
            request.KeepAlive = false; 
            request.ContentLength = fileStream.Length;

            using Stream requestStream = await request.GetRequestStreamAsync();
            await fileStream.CopyToAsync(requestStream);

            using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            return response.StatusCode == FtpStatusCode.ClosingData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Upload failed: {ex.Message}");
            return false;
        }
    }

    public static async Task<Stream> RetrieveFileAsync(string id)
    {
        try
        {
            string uri = $"ftp://{_ftp}/{id}";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = false;

            FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            MemoryStream output = new MemoryStream();
            using Stream responseStream = response.GetResponseStream();
            await responseStream.CopyToAsync(output);
            output.Position = 0;
            return output;
        }
        catch (WebException ex) when ((ex.Response as FtpWebResponse)?.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
        {
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Retrieve failed: {ex.Message}");
            return null;
        }
    }

    public static async Task<bool> FileExistsAsync(string id)
    {
        try
        {
            string uri = $"ftp://{_ftp}/{id}";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);
            request.UsePassive = true;
            request.KeepAlive = false;

            using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            return true;
        }
        catch (WebException ex) when ((ex.Response as FtpWebResponse)?.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
        {
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"File exists check failed: {ex.Message}");
            return false;
        }
    }
}