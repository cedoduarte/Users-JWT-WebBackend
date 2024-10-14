using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T?> CreateAsync(T entity, CancellationToken cancel = default)
        {
            var newEntity = await _dbContext.AddAsync(entity, cancel);
            await _dbContext.SaveChangesAsync(cancel);
            return newEntity.Entity;
        }

        public async Task<IEnumerable<T>> FindAllAsync(CancellationToken cancel = default)
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync(cancel);
        }

        public async Task<T?> FindOneAsync(int id, CancellationToken cancel = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => EF.Property<int>(entity, "Id") == id, cancel);
        }

        public async Task<T?> UpdateAsync(T entity, CancellationToken cancel = default)
        {
            var updatedEntity = _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancel);
            return updatedEntity.Entity;
        }

        public async Task<T?> RemoveAsync(int id, CancellationToken cancel = default)
        {
            var foundEntity = await _dbSet.FindAsync(new object[] { id }, cancel);
            if (foundEntity is null)
            {
                return null;
            }
            _dbSet.Remove(foundEntity);
            await _dbContext.SaveChangesAsync(cancel);
            return foundEntity;
        }
    }
}
