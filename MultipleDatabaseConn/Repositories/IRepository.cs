using MultipleDatabaseConn.Models;

namespace MultipleDatabaseConn.Repositories
{
    public interface IRepository<T>
    {
        Task CreateAsync(T entity);
        Task MultipleCreateAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<PagedResult<T>?> GetAllAsync(int pageNumber, int pageSize);
    }
}
