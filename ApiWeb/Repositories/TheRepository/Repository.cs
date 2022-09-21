using ApiWeb.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Repositories.TheRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext Context;
        public DbSet<T> DbSet { get; set; }
        public Repository(DbContext Context)
        {
            this.Context = Context;
            DbSet = Context.Set<T>();
        }
        private bool disposed = false;
        public List<T> GetAll() => DbSet.ToList();
        public async Task<List<T>> GetAllAsync() => await DbSet.ToListAsync();
        public async Task<T> GetOneIntIdAsync(int id) => await DbSet.FindAsync(id);
        public async Task<T> GetOneStringIdAsync(string id) => await DbSet.FindAsync(id);
        public async Task<T> GetOneGuidIdAsync(Guid id) => await DbSet.FindAsync(id);
        public async Task AddAsync(T entity) { await DbSet.AddAsync(entity); }
        public void Delete(T entity) { DbSet.Remove(entity); }
        public void DeleteAll() { DbSet.RemoveRange(DbSet); }
        public async Task SaveAsync() { await Context.SaveChangesAsync(); }
        public void Entry(T entity) { Context.Entry(entity).State = EntityState.Modified; }
        public void EntryUnchanged(T entity) { Context.Entry(entity).State = EntityState.Unchanged; }
        public void EntryDetached(T entity) { Context.Entry(entity).State = EntityState.Detached; }
        public void EntryAdded(T entity) { Context.Entry(entity).State = EntityState.Added; }
        public async Task DisposeAsync() { if (!disposed) { await Context.DisposeAsync(); disposed = true; } }

    }
}
