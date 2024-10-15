using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly AppDbContext _dbContext;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public RolePermissionService(AppDbContext dbContext, IRolePermissionRepository rolePermissionRepository)
        {
            _dbContext = dbContext;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<RolePermissionViewModel> CreateAsync(CreateRolePermissionDto createRolePermissionDto, CancellationToken cancel)
        {
            throw new NotImplementedException("This method is not implemented yet");
        }
        
        public async Task<IEnumerable<RolePermissionViewModel>> FindAllAsync(CancellationToken cancel)
        {
            throw new NotImplementedException("This method is not implemented yet");
        }

        public async Task<RolePermissionViewModel> UpdateAsync(UpdateRolePermissionDto updateRolePermissionDto, CancellationToken cancel)
        {
            throw new NotImplementedException("This method is not implemented yet");
        }
    }
}
