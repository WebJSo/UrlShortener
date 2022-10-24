
using LiteDB;
using Repos.Interfaces;
using Shared.Models;
using Shared.Interfaces;

namespace Repos
{
    /// <summary>
    /// ShortUrlRepo extends GenericRepo for more specific queries and provides concrete class ShortUrlItem
    /// </summary>
    public class ShortUrlRepo : GenericRepo<ShortUrlItem>, IShortUrlRepo
    {
        public ShortUrlRepo(ILiteDatabase db) : base(db) { }
        // get entity by short URL
        public IShortUrlItem GetByShortUrl(string shortUrl) => GetCollection.FindOne(f => f.ShortUrl == shortUrl);
    }
}
