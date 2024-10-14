using WebApplication1.DTOs;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<PermissionViewModel> CreateAsync(CreatePermissionDto createPermissionDto, CancellationToken cancel = default);
        Task<IEnumerable<PermissionViewModel>> FindAllAsync(CancellationToken cancel = default);
        Task<PermissionViewModel> FindOneAsync(int id, CancellationToken cancel = default);
        Task<PermissionViewModel> UpdateAsync(UpdatePermissionDto updatePermissionDto, CancellationToken cancel = default);
        Task<PermissionViewModel> SoftDeleteAsync(int id, CancellationToken cancel = default);
        Task<PermissionViewModel> RemoveAsync(int id, CancellationToken cancel = default);
    }
}
