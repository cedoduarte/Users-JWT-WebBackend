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
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _dbContext;

        public UserService(IMapper mapper, IUserRepository userRepository, AppDbContext dbContext)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _dbContext = dbContext;
        }

        public async Task<UserViewModel> CreateAsync(CreateUserDto createUserDto, CancellationToken cancel)
        {
            var newUser = _mapper.Map<User>(createUserDto);
            newUser.Created = DateTime.UtcNow;
            var createdUser = await _userRepository.CreateAsync(newUser, cancel);
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
                .FirstOrDefaultAsync(cancel);
            if (foundUser is null)
            {
                throw new Exception($"User Not Found, ID = {id}");
            }
            return _mapper.Map<UserViewModel>(foundUser);
        }

        public async Task<UserViewModel> UpdateAsync(UpdateUserDto updateUserDto, CancellationToken cancel)
        {
            var foundUser = await _userRepository.FindOneAsync(updateUserDto.Id, cancel);
            if (foundUser is null)
            {
                throw new Exception($"User Not Found, ID = {updateUserDto.Id}");
            }
            var user = _mapper.Map<User>(updateUserDto);
            user.Updated = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(user, cancel);
            return _mapper.Map<UserViewModel>(updatedUser);
        }

        public async Task<UserViewModel> SoftDeleteAsync(int id, CancellationToken cancel)
        {
            var foundUser = await _userRepository.FindOneAsync(id, cancel);
            if (foundUser is null)
            {
                throw new Exception($"User Not Found, ID = {id}");
            }
            foundUser.IsDeleted = true;
            foundUser.Deleted = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(foundUser, cancel);
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
