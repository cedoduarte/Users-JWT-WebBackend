using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;
        private readonly IRepository<RolePermission> _rolePermissionRepository;

        public RolePermissionService(IMapper mapper, AppDbContext dbContext, IRepository<RolePermission> rolePermissionRepository)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<RolePermissionViewModel> CreateAsync(CreateRolePermissionDto createRolePermissionDto, CancellationToken cancel)
        {
            var foundRole = await _dbContext.Roles
                .Where(x => x.Id == createRolePermissionDto.RoleId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundRole is null)
            {
                throw new NotFoundException($"Role Not Found, ID = {createRolePermissionDto.RoleId}");
            }
            var foundPermission = await _dbContext.Permissions
                .Where(x => x.Id == createRolePermissionDto.PermissionId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundPermission is null)
            {
                throw new NotFoundException($"Permission Not Found, ID = {createRolePermissionDto.PermissionId}");
            }
            var newRolePermission = _mapper.Map<RolePermission>(createRolePermissionDto);
            var createdRolePermission = await _rolePermissionRepository.CreateAsync(newRolePermission, cancel);
            var fullRolePermission = await _dbContext.RolePermissions
                .Where(x => x.Id == createdRolePermission!.Id)
                .Include(x => x.Role)
                .Include(x => x.Permission)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            return _mapper.Map<RolePermissionViewModel>(fullRolePermission);
        }
        
        public async Task<IEnumerable<RolePermissionViewModel>> FindAllAsync(CancellationToken cancel)
        {
            var rolePermissionList = await _dbContext.RolePermissions
                .Include(x => x.Role)
                .Include(x => x.Permission)
                .AsNoTracking()
                .ToListAsync(cancel);
            return _mapper.Map<IEnumerable<RolePermissionViewModel>>(rolePermissionList);
        }

        public async Task<RolePermissionViewModel> RemoveAsync(int roleId, int permissionId, CancellationToken cancel)
        {
            var foundRolePermission = await _dbContext.RolePermissions
                .Where(x => x.RoleId == roleId && x.PermissionId == permissionId)
                .Include (x => x.Role)
                .Include(x => x.Permission)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundRolePermission is null)
            {
                throw new NotFoundException($"Role-Permission Not Found");
            }
            await _rolePermissionRepository.RemoveAsync(foundRolePermission.Id, cancel);
            return _mapper.Map<RolePermissionViewModel>(foundRolePermission);
        }
    }
}
