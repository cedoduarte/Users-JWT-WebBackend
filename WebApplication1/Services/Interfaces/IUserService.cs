using WebApplication1.DTOs;
using WebApplication1.ViewModels;

namespace WebApplication1.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> CreateAsync(CreateUserDto createUserDto, CancellationToken cancel = default);
        Task<IEnumerable<UserViewModel>> FindAllAsync(CancellationToken cancel = default);
        Task<UserViewModel> FindOneAsync(int id, CancellationToken cancel = default);
        Task<UserViewModel> UpdateAsync(UpdateUserDto updateUserDto, CancellationToken cancel = default);
        Task<UserViewModel> SoftDeleteAsync(int id, CancellationToken cancel = default);
        Task<UserViewModel> RemoveAsync(int id, CancellationToken cancel = default);
    }
}
