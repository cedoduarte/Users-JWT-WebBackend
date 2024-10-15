using WebApplication1.Models;

namespace WebApplication1.Repositories.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<RolePermission?> CreateAsync(RolePermission rolePermission, CancellationToken cancel = default);
        Task<IEnumerable<RolePermission>> FindAllAsync(CancellationToken cancel = default);
        Task<RolePermission?> UpdateAsync(RolePermission rolePermission, CancellationToken cancel = default);
    }
}
