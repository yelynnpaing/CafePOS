
using CafePOS.Data;
using Microsoft.EntityFrameworkCore;

namespace CafePOS.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _context { get; set; }
        private DbSet<T> _dbset { get; set; }

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllByIdAsync<TKey>(TKey id, string propertyName, QueryOptions<T> options)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id, QueryOptions<T> options)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
