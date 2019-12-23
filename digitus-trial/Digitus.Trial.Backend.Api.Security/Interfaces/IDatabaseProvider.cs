using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digitus.Trial.Backend.Api.Interfaces
{
    public interface IDatabaseProvider<T> where T: class
    {
        Task<T> GetById(int id);
        Task<T> GetById(Guid id);
        Task<IList<T>> GetAllAsync();
        Task<T> Add(T item);
        Task<bool> AddRange(IList<T> items);
        Task<T> Update(T item);
        Task<T> Delete(T item);
        Task<T> DeleteById(int id);
        Task<T> DeleteById(Guid id);
        Task<IList<T>> GetByFilter(dynamic filter);
    }
}
