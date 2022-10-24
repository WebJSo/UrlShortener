using Shared.Models;
using System.Linq;
using Services.Interfaces;
using Services;
using Xunit;
using UnitTests.Repos;
using Repos.Interfaces;

namespace UnitTests
{
    /// <summary>
    /// Test IShortUrlService public methods against different scenarios using ShortUrlTestRepo for CRUD operations
    /// </summary>
    public class ShortUrlServiceTests
    {
        private readonly IShortUrlService _shortUrlService;        
        private readonly IShortUrlRepo _testRepo;

        public ShortUrlServiceTests()
        {
            _testRepo = new ShortUrlTestRepo();
            _shortUrlService = new ShortUrlService(_testRepo);
        }

        [Fact]
        public void TestGetUrlByIdMatchesShortUrl()
        {
            int id = 1;
            string url = "www.bbc.co.uk",
                   shortUrl = "test";

            // add new ShortUrlItem to test
            _testRepo.Add(new ShortUrlItem()
            {
                Id = id,
                ShortUrl = shortUrl,
                Url = string.Empty
            });

            // get added ShortUrlItem from service by id
            CreateUrlViewModel model = _shortUrlService.GetUrlById(id, url);

            // expect found item and not null
            Assert.NotNull(model);

            //  expect formatted host and short url
            Assert.Equal($"https://{url}/{shortUrl}", model.ShortUrl);
        }        

        [Fact]
        public void TestGetAllUrlsViewModelMatches()
        {
            int id = 1;
            string localHost = "localhost:123";

            // add new ShortUrlItem to test
            _testRepo.Add(new ShortUrlItem()
            {
                Id = id,
                ShortUrl = string.Empty,
                Url = string.Empty
            });

            // get view model and all ShortUrls 
            ManageUrlViewModel model = _shortUrlService.GetAllUrls(localHost);

            // expect model not null
            Assert.NotNull(model);

            // expect base url is formatted
            Assert.Equal($"https://{localHost}/", model.BaseUrl);

            // expect there are short urls in collection
            Assert.True(model.ShortUrls.Any());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestGetUrlByIdFindsNoResultsWhenInvalidId(int id)
        {
            // add new ShortUrlItem to test
            _testRepo.Add(new ShortUrlItem()
            {
                Id = 1,
                ShortUrl = string.Empty,
                Url = string.Empty
            });

            // get ShortUrlItem from service by id when using invalid or unavailable id
            CreateUrlViewModel model = _shortUrlService.GetUrlById(id, string.Empty);

            // expect no results
            Assert.True(model == null);
        }

        [Theory]
        [InlineData("www.bbc.co.uk", "test1", true)]
        [InlineData("www.bbc.com/123", "test2", true)]
        [InlineData("www.itv.co.uk?test=2", "test3;", true)]
        [InlineData("www.bbc2.co.uk?o=2&r=4", "test4", true)]
        [InlineData("www.bbc2123.co.uk", "test5", false)]
        [InlineData("www.bbc2123awd.co.uk/new", "test6", false)]
        public void TestGetRedirectUrlFindsCorrectUrlFromShortUrlIfAdded(string url, string shortUrl, bool add)
        {
            string redirectUrl = "/";

            // decide whether to add record for test
            if (add)
            {
                // add new ShortUrlItem to test
                _testRepo.Add(new ShortUrlItem()
                {
                    Id = 1,
                    ShortUrl = shortUrl,
                    Url = url
                });
            }

            // expect match when compare url from redirect url, when record not available expect default url
            Assert.Equal(add ? url : redirectUrl, _shortUrlService.GetRedirectUrl(shortUrl, redirectUrl));            
        }

        [Theory]
        [InlineData("www.bbc.co.uk")]
        [InlineData("www.google.co.uk/test")]
        [InlineData("www.hotmail.co.uk?test=123")]
        [InlineData("www.bing.co.uk?t=1&r=2")]
        [InlineData("www.itv.co.uk/t/r/?i=3")]
        public void TestAddUrlReturnsAnIdAboveZeroWithValidUrls(string url)
        {
            // expect all urls to be added based on return id not being 0
            Assert.True(_shortUrlService.AddUrl(url) > 0);
        }

        [Theory]
        [InlineData("wwwhttp://.itv.co.uk")]
        [InlineData("www.itv.c..o.uk")]
        public void TestAddUrlReturnsAnIdOfZeroWithInValidUrls(string url)
        {
            // expect no new urls to be added based on return id being 0 
            Assert.True(_shortUrlService.AddUrl(url) == 0);
        }

        [Theory]
        [InlineData("https://www.bbc.co.uk/1", "/update56", 1, 1)]
        [InlineData("https://www.bbc.com/test", "/update34?ed=1", 1, 1)]
        [InlineData("https://www.itv.co.uk", "/update12?ed=1&test=t", 1, 1)]
        [InlineData("https://www.bbc2.co.uk", "/update23/dir", 1, 1)]
        [InlineData("https://www.bbc.co.uk/1", "/update56", 1, 2)]
        [InlineData("https://www.bbc.com/test", "/update34?ed=1", 1, 2)]
        public void TestUpdateUrlUpdatesIfCorrectIdElseDoesNotUpdate(string url, string update, int id1, int id2)
        {
            string newUrl = url + update;
            bool shouldUpdate = id1 == id2;

            // add new ShortUrlItem to test with first id
            _testRepo.Add(new ShortUrlItem()
            {
                Id = id1,
                ShortUrl = string.Empty,
                Url = url
            });

            // update to new url with second Id           
            bool hasUpdated = _shortUrlService.UpdateUrl(id2, newUrl);

            // get by id with first id and check updated url or original url based on shouldUpdate
            Assert.Equal(shouldUpdate ? newUrl : url, _testRepo.GetById(id1).Url);

            // expect hasUpdated to be false
            Assert.Equal(shouldUpdate, hasUpdated);
        }
    }
}
