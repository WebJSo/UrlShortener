using Shared.Models;
using Shared.Interfaces;

namespace Repos.Interfaces
{
    public interface IShortUrlRepo : IGenericRepo<ShortUrlItem>
    {
        IShortUrlItem GetByShortUrl(string shortUrl);
    }
}
