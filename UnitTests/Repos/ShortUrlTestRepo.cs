using Repos.Interfaces;
using Shared.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Repos
{
    /// <summary>
    /// Test repository providing all IShortUrlRepo CRUD operations for testing purposes
    /// </summary>
    public class ShortUrlTestRepo : IShortUrlRepo
    {
        // store collection
        private List<ShortUrlItem> _dbItems = new List<ShortUrlItem>();

        // get all entities
        public IEnumerable<ShortUrlItem> GetAll() => _dbItems;

        // get entity by Id
        public ShortUrlItem GetById(int id) => _dbItems.SingleOrDefault(s => s.Id == id);

        // add new entity
        public int Add(ShortUrlItem item)
        {
            _dbItems.Add(item);
            item.Id = _dbItems.Count;
            return item.Id;
        }

        // update entity - updates will be performed on _dbItems/item in calling class
        public bool Update(ShortUrlItem item) => true;

        // remove entity
        public bool Remove(int id) => throw new NotImplementedException();

        // get entity by short url
        public IShortUrlItem GetByShortUrl(string shortUrl) => _dbItems.SingleOrDefault(f => f.ShortUrl == shortUrl);
    }
}
