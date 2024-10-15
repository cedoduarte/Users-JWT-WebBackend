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
    public class PermissionService : IPermissionService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly AppDbContext _dbContext;

        public PermissionService(IMapper mapper, IRepository<Permission> permissionRepository, AppDbContext dbContext)
        {
            _mapper = mapper;
            _permissionRepository = permissionRepository;
            _dbContext = dbContext;
        }

        public async Task<PermissionViewModel> CreateAsync(CreatePermissionDto createPermissionDto, CancellationToken cancel)
        {
            var newPermission = _mapper.Map<Permission>(createPermissionDto);
            newPermission.Created = DateTime.UtcNow;
            var createdPermission = await _permissionRepository.CreateAsync(newPermission, cancel);
            return _mapper.Map<PermissionViewModel>(createdPermission);
        }

        public async Task<IEnumerable<PermissionViewModel>> FindAllAsync(CancellationToken cancel)
        {
            var permissionList = await _dbContext.Permissions
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancel);
            return _mapper.Map<IEnumerable<PermissionViewModel>>(permissionList);
        }

        public async Task<PermissionViewModel> FindOneAsync(int id, CancellationToken cancel)
        {
            var foundPermission = await _dbContext.Permissions
                .Where(x => !x.IsDeleted && x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundPermission is null)
            {
                throw new NotFoundException($"Permission Not Found, ID = {id}");
            }
            return _mapper.Map<PermissionViewModel>(foundPermission);
        }

        public async Task<PermissionViewModel> UpdateAsync(UpdatePermissionDto updatePermissionDto, CancellationToken cancel)
        {
            var foundPermission = await _dbContext.Permissions
                .Where(x => !x.IsDeleted && x.Id == updatePermissionDto.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundPermission is null)
            {
                throw new NotFoundException($"Permission Not Found, ID = {updatePermissionDto.Id}");
            }
            var permission = _mapper.Map<Permission>(updatePermissionDto);
            permission.Updated = DateTime.UtcNow;
            var updatedPermission = await _permissionRepository.UpdateAsync(permission, cancel);
            return _mapper.Map<PermissionViewModel>(updatedPermission);
        }

        public async Task<PermissionViewModel> SoftDeleteAsync(int id, CancellationToken cancel)
        {
            var foundPermission = await _dbContext.Permissions
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync(cancel);
            if (foundPermission is null)
            {
                throw new NotFoundException($"Permission Not Found, ID = {id}");
            }
            foundPermission.IsDeleted = true;
            foundPermission.Deleted = DateTime.UtcNow;
            var updatedPermission = await _permissionRepository.UpdateAsync(foundPermission, cancel);
            return _mapper.Map<PermissionViewModel>(updatedPermission);
        }

        public async Task<PermissionViewModel> RemoveAsync(int id, CancellationToken cancel)
        {
            var foundPermission = await _permissionRepository.FindOneAsync(id, cancel);
            if (foundPermission is null)
            {
                throw new NotFoundException($"Permission Not Found, {id}");
            }
            var removedPermission = await _permissionRepository.RemoveAsync(id, cancel);
            return _mapper.Map<PermissionViewModel>(removedPermission);
        }
    }
}
