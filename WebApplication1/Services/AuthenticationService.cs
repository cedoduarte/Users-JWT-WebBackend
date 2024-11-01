using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Interfaces;
using WebApplication1.Shared;
using WebApplication1.Utilities;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;
        private readonly IRepository<Authentication> _authenticationRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            IMapper mapper,
            AppDbContext dbContext,
            IRepository<Authentication> authenticationRepository,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _authenticationRepository = authenticationRepository;
            _configuration = configuration;
        }

        public async Task<(AuthenticationViewModel, string)> AuthenticateAsync(AuthenticateDto authenticateDto, CancellationToken cancel)
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

            var createdAuthentication = await _authenticationRepository.CreateAsync(new Authentication()
            {
                UserId = foundUser.Id,
                Date = DateTime.UtcNow
            }, cancel);

            return (_mapper.Map<AuthenticationViewModel>(createdAuthentication), 
                    GetJwtToken(foundUser.Id, Constants.Jwt.TokenExpirationSeconds));
        }

        public string GetJwtToken(int userId, int expirationSeconds)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Constants.Jwt.UserIdClaim, $"{userId}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"]!,
                _configuration["Jwt:Audience"]!,
                claims,
                expires: DateTime.UtcNow.AddSeconds(expirationSeconds),
                signingCredentials: signIn);

            string jwtGeneratedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtGeneratedToken;
        }
    }
}