using Shared.Models;

namespace Services.Interfaces
{
    public interface IShortUrlService
    {
        CreateUrlViewModel GetUrlById(int id, string host);
        ManageUrlViewModel GetAllUrls(string host);
        string GetRedirectUrl(string shortUrl, string redirectUrl);
        int AddUrl(string url);
        bool UpdateUrl(int id, string url);
        bool RemoveUrl(int id);
    }
}
