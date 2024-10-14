using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRoleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserRole?> CreateAsync(UserRole userRole, CancellationToken cancel)
        {
            var newUserRole = await _dbContext.AddAsync(userRole, cancel);
            await _dbContext.SaveChangesAsync(cancel);
            return newUserRole.Entity;
        }

        public async Task<IEnumerable<UserRole>> FindAllAsync(CancellationToken cancel)
        {
            return await _dbContext.UserRoles.AsNoTracking().ToListAsync(cancel);
        }

        public async Task<UserRole?> FindOneByUserIdAsync(int userId, CancellationToken cancel)
        {
            var foundUserRole = await _dbContext.UserRoles
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            return foundUserRole;
        }

        public async Task<UserRole?> UpdateAsync(UserRole userRole, CancellationToken cancel)
        {
            var updatedUserRole = _dbContext.Update(userRole);
            await _dbContext.SaveChangesAsync(cancel);
            return updatedUserRole.Entity;
        }

        public async Task<UserRole?> RemoveByUserIdAsync(int userId, CancellationToken cancel)
        {
            var foundUserRole = await _dbContext.UserRoles
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundUserRole is null)
            {
                return null;
            }
            _dbContext.UserRoles.Remove(foundUserRole);
            await _dbContext.SaveChangesAsync(cancel);
            return foundUserRole;
        }
    }
}
