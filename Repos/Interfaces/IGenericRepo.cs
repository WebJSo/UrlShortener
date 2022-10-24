using System.Collections.Generic;

namespace Repos.Interfaces
{
    public interface IGenericRepo<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        int Add(T item);
        bool Update(T item);
        bool Remove(int id);
    }
}
