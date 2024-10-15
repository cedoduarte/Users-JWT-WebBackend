using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly AppDbContext _dbContext;

        public RolePermissionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RolePermission?> CreateAsync(RolePermission rolePermission, CancellationToken cancel)
        {
            var newRolePermission = await _dbContext.AddAsync(rolePermission, cancel);
            await _dbContext.SaveChangesAsync(cancel);
            return newRolePermission.Entity;
        }

        public async Task<IEnumerable<RolePermission>> FindAllAsync(CancellationToken cancel)
        {
            return await _dbContext.RolePermissions.AsNoTracking().ToListAsync(cancel);
        }

        public async Task<RolePermission?> UpdateAsync(RolePermission rolePermission, CancellationToken cancel)
        {
            var updatedRolePermission = _dbContext.Update(rolePermission);
            await _dbContext.SaveChangesAsync(cancel);
            return updatedRolePermission.Entity;
        }
    }
}
