using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAppDbContext _dbContext;

        public AuthorizationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasPermissionAsync(string userIdString, string requiredPermission, CancellationToken cancel)
        {
            if (!int.TryParse(userIdString, out int userId))
            {
                return false;
            }

            var hasPermission = await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_dbContext.RolePermissions,
                      ur => ur.RoleId,
                      rp => rp.RoleId,
                      (ur, rp) => rp.PermissionId)
                .Join(_dbContext.Permissions,
                      rpPermissionId => rpPermissionId,
                      p => p.Id,
                      (rpPermissionId, p) => p)
                .AnyAsync(p => p.Name == requiredPermission && !p.IsDeleted, cancel);

            return hasPermission;
        }
    }
}
