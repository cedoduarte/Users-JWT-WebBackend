namespace WebApplication1.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> CreateAsync(T entity, CancellationToken cancel = default);
        Task<IEnumerable<T>> FindAllAsync(CancellationToken cancel = default);
        Task<T?> FindOneAsync(int id, CancellationToken cancel = default);
        Task<T?> UpdateAsync(T entity, CancellationToken cancel = default);
        Task<T?> RemoveAsync(int id, CancellationToken cancel = default);
    }
}
