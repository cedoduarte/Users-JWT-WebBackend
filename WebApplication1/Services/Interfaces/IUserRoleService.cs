using WebApplication1.DTOs;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleViewModel>> FindAllAsync(CancellationToken cancel = default);
        Task<UserRoleViewModel?> FindOneByUserIdAsync(int userId, CancellationToken cancel = default);
        Task<UserRoleViewModel> UpdateAsync(UpdateUserRoleDto updateUserRoleDto, CancellationToken cancel = default);
    }
}
