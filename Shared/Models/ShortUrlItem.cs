using Shared.Interfaces;

namespace Shared.Models
{
    public class ShortUrlItem : IShortUrlItem
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
    }
}
