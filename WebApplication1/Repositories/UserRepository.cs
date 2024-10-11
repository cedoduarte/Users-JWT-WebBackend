using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IUserRepository
    {
        Task<User?> CreateAsync(User user, CancellationToken cancel = default);
        Task<IEnumerable<User>> FindAllAsync(CancellationToken cancel = default);
        Task<User?> FindOneAsync(int id, CancellationToken cancel = default);
        Task<User?> UpdateAsync(User user, CancellationToken cancel = default);
        Task<User?> RemoveAsync(int id, CancellationToken cancel = default);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> CreateAsync(User user, CancellationToken cancel)
        {
            var newUser = await _dbContext.AddAsync(user, cancel);
            await _dbContext.SaveChangesAsync(cancel);
            return newUser.Entity;
        }

        public async Task<IEnumerable<User>> FindAllAsync(CancellationToken cancel)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .ToListAsync(cancel);
        }

        public async Task<User?> FindOneAsync(int id, CancellationToken cancel)
        {
            var foundUser = await _dbContext.Users
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            return foundUser;
        }
        
        public async Task<User?> UpdateAsync(User user, CancellationToken cancel)
        {            
            var updatedUser = _dbContext.Update(user);
            await _dbContext.SaveChangesAsync(cancel);
            return updatedUser.Entity;
        }

        public async Task<User?> RemoveAsync(int id, CancellationToken cancel)
        {
            var foundUser = await _dbContext.Users.FindAsync(id, cancel);
            if (foundUser is null)
            {
                return null;                
            }
            _dbContext.Users.Remove(foundUser);
            await _dbContext.SaveChangesAsync(cancel);
            return foundUser;
        }
    }
}
