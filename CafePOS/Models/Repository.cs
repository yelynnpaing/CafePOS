
using CafePOS.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime;

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

        //public Task DeleteAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public Task<IEnumerable<T>> GetAllByIdAsync<TKey>(TKey id, string propertyName, QueryOptions<T> options)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(Guid id, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbset;
            if (options.HasWhere)
            {
                query = query.Where(options.Where);
            }
            if (options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }
            foreach(string include in options.GetIncludes())
            {
                query = query.Include(include);
            }

            var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            string primaryKeyName = key?.Name;
            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, primaryKeyName) == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            T entity = await _dbset.FindAsync(id);
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
