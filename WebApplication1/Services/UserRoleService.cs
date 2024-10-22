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
    public class UserRoleService : IUserRoleService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IMapper mapper, AppDbContext dbContext, IUserRoleRepository userRoleRepository)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IEnumerable<UserRoleViewModel>> FindAllAsync(CancellationToken cancel)
        {
            var userRoleList = await _dbContext.UserRoles
                .AsNoTracking()
                .ToListAsync(cancel);
            var result = new List<UserRoleViewModel>();
            foreach (var userRole in userRoleList)
            {
                var foundUser = await _dbContext.Users
                    .Where(x => !x.IsDeleted && x.Id == userRole.UserId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancel);
                if (foundUser is not null)
                {
                    var foundRole = await _dbContext.Roles
                        .Where(x => !x.IsDeleted && x.Id == userRole.RoleId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cancel);
                    if (foundRole is not null)
                    {
                        var userRoleViewModel = new UserRoleViewModel()
                        {
                            User = _mapper.Map<UserViewModel>(foundUser),
                            Role = _mapper.Map<RoleViewModel>(foundRole)
                        };
                        result.Add(userRoleViewModel);
                    }
                }
            }
            return result;
        }

        public async Task<UserRoleViewModel?> FindOneByUserIdAsync(int userId, CancellationToken cancel)
        {
            var foundUser = await _dbContext.Users
                .Where(x => !x.IsDeleted && x.Id == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundUser is null)
            {
                throw new NotFoundException($"User Not Found, ID = {userId}");
            }
            var foundUserRole = await _dbContext.UserRoles
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundUserRole is not null)
            {
                var foundRole = await _dbContext.Roles
                    .Where(x => !x.IsDeleted && x.Id == foundUserRole.RoleId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancel);
                if (foundRole is null)
                {
                    throw new NotFoundException($"Role Not Found, ID = {foundUserRole.RoleId}");
                }
                var userRoleViewModel = new UserRoleViewModel()
                {
                    User = _mapper.Map<UserViewModel>(foundUser),
                    Role = _mapper.Map<RoleViewModel>(foundRole)
                };
                return userRoleViewModel;
            }
            return null;
        }

        public async Task<UserRoleViewModel> UpdateAsync(UpdateUserRoleDto updateUserRoleDto, CancellationToken cancel)
        {
            var foundUser = await _dbContext.Users
                .Where(x => !x.IsDeleted && x.Id == updateUserRoleDto.UserId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundUser is null)
            {
                throw new NotFoundException($"User Not Found, ID = {updateUserRoleDto.UserId}");
            }
            var foundRole = await _dbContext.Roles
                .Where(x => !x.IsDeleted && x.Id == updateUserRoleDto.RoleId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundRole is null)
            {
                throw new NotFoundException($"Role Not Found, ID = {updateUserRoleDto.RoleId}");
            }
            var foundUserRole = await _dbContext.UserRoles
                .Where(x => x.UserId == updateUserRoleDto.UserId)
                .FirstOrDefaultAsync(cancel);
            if (foundUserRole is null)
            {
                throw new NotFoundException($"User-Role Not Found, User ID = {updateUserRoleDto.UserId}");
            }
            foundUserRole.UserId = updateUserRoleDto.UserId ?? 0;
            foundUserRole.RoleId = updateUserRoleDto.RoleId ?? 0;
            var updatedUserRole = await _userRoleRepository.UpdateAsync(foundUserRole, cancel);
            var userRoleViewModel = new UserRoleViewModel()
            {
                User = _mapper.Map<UserViewModel>(foundUser),
                Role = _mapper.Map<RoleViewModel>(foundRole)
            };
            return userRoleViewModel;
        }
    }
}
