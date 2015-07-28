namespace NZSchools.DataParser.Services.WebClientService
{
    public interface IWebClientService
    {
        bool DownloadFile(string url, string filePath);
    }
}
