using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.DatabaseContext;
using System.Linq.Expressions;
using UserManagement.Application.Repositories;

namespace UserManagement.Infrastructure.Repositories
{
    public class GenericRepositoryImplementation<T> : GenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepositoryImplementation(ApplicationContext dbContext)
        {
            this._context = dbContext;
            _dbSet = _context.Set<T>();
        }
        public T Delete(T entity)
        {
            return _dbSet.Remove(entity).Entity;
        }

        public async Task<T?> DeleteAsync(int id)
        {
            T? entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            return entity;
        }

        public List<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null, string includedProperties = "")
        {
            IQueryable<T> data = _dbSet;
            if (filter != null)
            {
                data = data.Where(filter);
            }

            foreach (string includedProperty in includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                data = data.Include(includedProperty);
            }

            if (ordering != null)
            {
                return ordering(data).ToList<T>();
            }

            return data.ToList();
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null, string includedProperties = "")
        {
            IQueryable<T> data = _dbSet;
            if (filter != null)
            {
                data = data.Where(filter);
            }

            foreach (string includedProperty in includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                data = data.Include(includedProperty);
            }

            if (ordering != null)
            {
                return await ordering(data).ToListAsync();
            }

            return await data.ToListAsync();
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Insert(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public async Task<T> InsertAsync(T entity)
        {
            var entityEntry = await _dbSet.AddAsync(entity);
            return entityEntry.Entity;
        }

        public T Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
