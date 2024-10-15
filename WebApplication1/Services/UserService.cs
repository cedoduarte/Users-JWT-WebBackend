using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly AppDbContext _dbContext;

        public UserService(IMapper mapper, 
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IUserRoleRepository userRoleRepository,
            AppDbContext dbContext)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _dbContext = dbContext;
        }

        public async Task<UserViewModel> CreateAsync(CreateUserDto createUserDto, CancellationToken cancel)
        {
            var newUser = _mapper.Map<User>(createUserDto);
            newUser.Created = DateTime.UtcNow;
            var createdUser = await _userRepository.CreateAsync(newUser, cancel);
            var foundRole = await _roleRepository.FindOneAsync(createUserDto.RoleId, cancel);
            var newUserRole = new UserRole()
            {
                UserId = createdUser!.Id,
                RoleId = foundRole!.Id
            };
            await _userRoleRepository.CreateAsync(newUserRole);            
            return _mapper.Map<UserViewModel>(createdUser);
        }

        public async Task<IEnumerable<UserViewModel>> FindAllAsync(CancellationToken cancel)
        {
            var userList = await _dbContext.Users
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancel);
            return _mapper.Map<IEnumerable<UserViewModel>>(userList);
        }

        public async Task<UserViewModel> FindOneAsync(int id, CancellationToken cancel)
        {
            var foundUser = await _dbContext.Users
                .Where(x => !x.IsDeleted && x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundUser is null)
            {
                throw new Exception($"User Not Found, ID = {id}");
            }
            return _mapper.Map<UserViewModel>(foundUser);
        }

        public async Task<UserViewModel> UpdateAsync(UpdateUserDto updateUserDto, CancellationToken cancel)
        {
            var foundUser = await _dbContext.Users
                .Where(x => !x.IsDeleted && x.Id == updateUserDto.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundUser is null)
            {
                throw new Exception($"User Not Found, ID = {updateUserDto.Id}");
            }
            var foundRole = await _dbContext.Roles
                .Where(x => !x.IsDeleted && x.Id == updateUserDto.RoleId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundRole is null)
            {
                throw new Exception($"Role Not Found, ID = {updateUserDto.RoleId}");
            }
            var foundUserRole = await _dbContext.UserRoles
                .Where(x => x.UserId == foundUser.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundUserRole is null)
            {
                throw new Exception($"Role Not Found, ID = {updateUserDto.RoleId}");
            }
            foundUser = _mapper.Map<User>(updateUserDto);
            foundUser.Updated = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(foundUser, cancel);
            foundUserRole.RoleId = updateUserDto.RoleId;
            await _userRoleRepository.UpdateAsync(foundUserRole, cancel);
            return _mapper.Map<UserViewModel>(updatedUser);
        }

        public async Task<UserViewModel> SoftDeleteAsync(int id, CancellationToken cancel)
        {
            var foundUser = await _dbContext.Users
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync(cancel);
            if (foundUser is null)
            {
                throw new Exception($"User Not Found, ID = {id}");
            }
            foundUser.IsDeleted = true;
            foundUser.Deleted = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(foundUser, cancel);
            await _userRoleRepository.RemoveByUserIdAsync(foundUser.Id);
            return _mapper.Map<UserViewModel>(updatedUser);
        }

        public async Task<UserViewModel> RemoveAsync(int id, CancellationToken cancel)
        {
            var foundUser = await _userRepository.FindOneAsync(id, cancel);
            if (foundUser is null)
            {
                throw new Exception($"User Not Found, ID = {id}");
            }
            var removedUser = await _userRepository.RemoveAsync(id, cancel);
            return _mapper.Map<UserViewModel>(removedUser);
        }
    }
}
