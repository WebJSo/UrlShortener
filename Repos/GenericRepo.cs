using LiteDB;
using Repos.Interfaces;
using System.Collections.Generic;

namespace Repos
{
    /// <summary>
    /// Abstract Generic Repository provides generic base functionality for all CRUD operations using LightDB
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly ILiteDatabase _db;
        private ILiteCollection<T> _collection;

        public GenericRepo(ILiteDatabase db) => _db = db;

        // get ILiteCollection
        protected ILiteCollection<T> GetCollection
        {
            get
            {
                if (_collection == null)
                    _collection = _db.GetCollection<T>(nameof(T));

                return _collection;
            }
        }

        // get all entities
        public IEnumerable<T> GetAll() => GetCollection.FindAll();

        // get entity by Id
        public T GetById(int id) => GetCollection.FindById(id);

        // add new entity
        public int Add(T item) => GetCollection.Insert(item);

        // update entity
        public bool Update(T item) => GetCollection.Update(item);

        // remove entity
        public bool Remove(int id) => GetCollection.Delete(id);
    }
}
