using MultipleDatabaseConn.Models;

namespace MultipleDatabaseConn.Repositories
{
    public interface IRepository<T>
    {
        Task Create(T entity);
        Task MultipleCreate(List<T> entities);
        Task Update(T entity);
        Task Delete(T entity);
        Task<T?> GetById(Guid id);
        Task<PagedResult<T>> GetAll(int pageNumber, int pageSize);
    }
}
