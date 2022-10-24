using System.Collections.Generic;
using Shared.Interfaces;

namespace Shared.Models
{
    public class ManageUrlViewModel
    {
        public string BaseUrl { get; set; }
        public IEnumerable<IShortUrlItem> ShortUrls { get; set; }
    }
}
