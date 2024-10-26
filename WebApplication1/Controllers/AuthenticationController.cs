using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private const int TokenExpirationSeconds = 86400; // 24 hours
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        public AuthenticationController(IConfiguration configuration, 
            ILogger<AuthenticationController> logger,
            IAuthenticationService authenticationService,
            IUserService userService,
            IUserRoleService userRoleService)
        {
            _configuration = configuration;
            _logger = logger;
            _authenticationService = authenticationService;
            _userService = userService;
            _userRoleService = userRoleService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateDto authenticateDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"AuthenticateDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                var authentication = await _authenticationService.AuthenticateAsync(authenticateDto, cancel);
                string token = GetJwtToken(authenticateDto.Username!, await GetRoleByUsername(authenticateDto.Username!), TokenExpirationSeconds);                
                var authenticatedDto = new AuthenticatedDto()
                {
                    Token = token,
                    Authentication = authentication
                };
                return Ok(authenticatedDto);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Resource Not Found, {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (AuthenticationException ex)
            {
                _logger.LogWarning($"Authentication failed for user: {authenticateDto.Username}. Reason: {ex.Message}");
                return Unauthorized("Authentication failed. Please check your credentials.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while authenticating user: {authenticateDto.Username}. Reason: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        private async Task<string> GetRoleByUsername(string username)
        {
            var foundUser = await _userService.FindOneAsync(username);
            var foundUserRole = await _userRoleService.FindOneByUserIdAsync(foundUser.Id);
            string role = foundUserRole!.Role!.Name!;
            return role;
        }

        private string GetJwtToken(string username, string role, int expirationSeconds)
        {
            var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Username", username),
                    new Claim(ClaimTypes.Role, role)
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
