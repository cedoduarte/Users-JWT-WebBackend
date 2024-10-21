using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticateDto authenticateDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"AuthenticateDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _authenticationService.AuthenticateAsync(authenticateDto, cancel));
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
    }
}
