using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;
using WebApplication1.Utilities;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;
        private readonly IRepository<Authentication> _authenticationRepository;

        public AuthenticationService(IMapper mapper, AppDbContext dbContext, IRepository<Authentication> authenticationRepository)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _authenticationRepository = authenticationRepository;
        }

        public async Task<AuthenticationViewModel> AuthenticateAsync(AuthenticateDto authenticateDto, CancellationToken cancel)
        {
            string passwordHash = Util.GetSha256Hash(authenticateDto.Password!);
            var foundUser = await _dbContext.Users
                .Where(x => string.Equals(x.Username, authenticateDto.Username) 
                         && string.Equals(x.PasswordHash, passwordHash))
                .AsNoTracking()
                .FirstOrDefaultAsync(cancel);
            if (foundUser is null)
            {
                throw new AuthenticationException("Invalid credentials");
            }
            var newAuthentication = new Authentication()
            {
                UserId = foundUser.Id,
                Date = DateTime.UtcNow
            };
            var createdAuthentication = await _authenticationRepository.CreateAsync(newAuthentication, cancel);
            return _mapper.Map<AuthenticationViewModel>(createdAuthentication);
        }
    }
}
