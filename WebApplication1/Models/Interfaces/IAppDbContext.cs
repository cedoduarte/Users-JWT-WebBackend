using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Permission> Permissions { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<Authentication> Authentications { get; set; }
    }
}
