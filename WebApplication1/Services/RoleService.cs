using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly AppDbContext _dbContext;

        public RoleService(IMapper mapper, IRoleRepository roleRepository, AppDbContext dbContext)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _dbContext = dbContext;
        }

        public async Task<RoleViewModel> CreateAsync(CreateRoleDto createRoleDto, CancellationToken cancel)
        {
            var newRole = _mapper.Map<Role>(createRoleDto);
            newRole.Created = DateTime.UtcNow;
            var createdRole = await _roleRepository.CreateAsync(newRole, cancel);
            return _mapper.Map<RoleViewModel>(createdRole);
        }

        public async Task<IEnumerable<RoleViewModel>> FindAllAsync(CancellationToken cancel)
        {
            var roleList = await _dbContext.Roles
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancel);
            return _mapper.Map<IEnumerable<RoleViewModel>>(roleList);
        }

        public async Task<RoleViewModel> FindOneAsync(int id, CancellationToken cancel)
        {
            var foundRole = await _dbContext.Roles
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync(cancel);
            if (foundRole is null)
            {
                throw new Exception($"Role Not Found, ID = {id}");
            }
            return _mapper.Map<RoleViewModel>(foundRole);
        }

        public async Task<RoleViewModel> UpdateAsync(UpdateRoleDto updateRoleDto, CancellationToken cancel)
        {
            var foundRole = await _roleRepository.FindOneAsync(updateRoleDto.Id, cancel);
            if (foundRole is null)
            {
                throw new Exception($"Role Not Found, ID = {updateRoleDto.Id}");
            }
            var role = _mapper.Map<Role>(updateRoleDto);
            role.Updated = DateTime.UtcNow;
            var updatedRole = await _roleRepository.UpdateAsync(role, cancel);
            return _mapper.Map<RoleViewModel>(updatedRole);
        }

        public async Task<RoleViewModel> SoftDeleteAsync(int id, CancellationToken cancel)
        {
            var foundRole = await _roleRepository.FindOneAsync(id, cancel);
            if (foundRole is null)
            {
                throw new Exception($"Role Not Found, ID = {id}");
            }
            foundRole.IsDeleted = true;
            foundRole.Deleted = DateTime.UtcNow;
            var updatedRole = await _roleRepository.UpdateAsync(foundRole, cancel);
            return _mapper.Map<RoleViewModel>(updatedRole);
        }

        public async Task<RoleViewModel> RemoveAsync(int id, CancellationToken cancel)
        {
            var foundRole = await _roleRepository.FindOneAsync(id, cancel);
            if (foundRole is null)
            {
                throw new Exception($"Role Not Found, ID = {id}");
            }
            var removedRole = await _roleRepository.RemoveAsync(id, cancel);
            return _mapper.Map<RoleViewModel>(removedRole);
        }
    }
}
