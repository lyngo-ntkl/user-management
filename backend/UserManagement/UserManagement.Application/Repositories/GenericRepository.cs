using UserManagement.Domain.Entities;
using System.Linq.Expressions;

namespace UserManagement.Application.Repositories
{
    public interface GenericRepository<T> where T: BaseEntity
    {
        T Delete(T entity);
        Task<T?> DeleteAsync(int id);
        List<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null, string includedProperties = "");
        Task<List<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null, string includedProperties = "");
        T? GetById(int id);
        Task<T?> GetByIdAsync(int id);
        T Insert(T entity);
        Task<T> InsertAsync(T entity);
        T Update(T entity);
    }
}
