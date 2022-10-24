using Repos.Interfaces;
using Services.Interfaces;
using Shared.Extensions;
using Shared.Models;
using Shared.Interfaces;

namespace Services
{
    /// <summary>
    /// ShortUrlService provides methods for utilising the database CRUD operations using the IShortUrlRepository
    /// Returns viewmodels for creating, loading and removing ShortUrlItems
    /// Utilises ShortUrlExtensions methods to format, validate and create short urls
    /// </summary>
    public class ShortUrlService : IShortUrlService
    {
        private readonly IShortUrlRepo _shortUrlRepository;

        public ShortUrlService(IShortUrlRepo shortUrlRepository) => _shortUrlRepository = shortUrlRepository;

        public CreateUrlViewModel GetUrlById(int id, string host)
        {
            // no records exist
            if (id == 0)
                return null;

            // get record by id
            IShortUrlItem item = _shortUrlRepository.GetById(id);

            // cant find record by id
            if (item == null)
                return null;

            // create correct/full short url for redirect page
            CreateUrlViewModel viewModel = new CreateUrlViewModel()
            {
                ShortUrl = host.FormatUrl() + "/" + item.ShortUrl,
                Url = item.Url
            };

            return viewModel;
        }

        public ManageUrlViewModel GetAllUrls(string host)
        {
            // populate BaseUrl to later combine with ShortUrls for valid short url
            ManageUrlViewModel viewModel = new ManageUrlViewModel()
            {
                BaseUrl = host.FormatUrl() + "/",
                ShortUrls = _shortUrlRepository.GetAll()
            };

            return viewModel;
        }

        public string GetRedirectUrl(string shortUrl, string defaultRedirectUrl)
        {
            // redirect back to default page
            if (string.IsNullOrEmpty(shortUrl))
                return defaultRedirectUrl;

            // get url from short url or return default page
            string redirectUrl = _shortUrlRepository.GetByShortUrl(shortUrl)?.Url ?? defaultRedirectUrl;

            // return correct redirect url
            return redirectUrl;
        }

        public int AddUrl(string url)
        {
            // format url
            string formattedUrl = url.FormatUrl();

            // cant add invalid url
            if (!formattedUrl.ValidUrl())
                return 0;

            ShortUrlItem item = new ShortUrlItem();

            // populate item to save
            item.Url = formattedUrl;

            // add item so auto-generated unique Id can be used
            _shortUrlRepository.Add(item);

            // something went wrong
            if (item.Id == 0)
                return 0;

            // generate shortUrl from Id
            item.ShortUrl = item.Id.ShortUrl();

            // update db with shortUrl
            bool hasUpdated = _shortUrlRepository.Update(item);

            // return id of newly added item if updated else 0
            return hasUpdated ? item.Id : 0;
        }

        public bool UpdateUrl(int id, string url)
        {
            // format url
            string formattedUrl = url.FormatUrl();

            // cant update to invalid url
            if (!formattedUrl.ValidUrl())
                return false;

            // get record by id to be updated
            ShortUrlItem item = _shortUrlRepository.GetById(id);

            // cant find record by id
            if (item == null)
                return false;

            // update with new url
            item.Url = formattedUrl;

            // update db 
            bool hasUpdated = _shortUrlRepository.Update(item);

            // return update status
            return hasUpdated;
        }

        public bool RemoveUrl(int id)
        {
            bool hasRemoved = _shortUrlRepository.Remove(id);

            // return removed status
            return hasRemoved;
        }
    }
}
