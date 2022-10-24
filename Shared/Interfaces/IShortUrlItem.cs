
namespace Shared.Interfaces
{
    public interface IShortUrlItem
    {
        int Id { get; set; }
        string Url { get; set; }
        string ShortUrl { get; set; }
    }
}
