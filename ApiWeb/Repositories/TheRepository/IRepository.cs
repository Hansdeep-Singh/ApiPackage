using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWeb.Repositories.TheRepository
{
    public interface IRepository<T>
    {
        Task AddAsync(T entity);
        void Delete(T entity);
        void DeleteAll();
        Task DisposeAsync();
        void Entry(T entity);
        void EntryAdded(T entity);
        void EntryDetached(T entity);
        void EntryUnchanged(T entity);
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        Task<T> GetOneGuidIdAsync(Guid id);
        Task<T> GetOneIntIdAsync(int id);
        Task<T> GetOneStringIdAsync(string id);
        Task SaveAsync();
    }
}