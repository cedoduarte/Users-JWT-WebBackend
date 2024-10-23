using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserRoleController : Controller
    {
        private readonly ILogger<UserRoleController> _logger;
        private readonly IUserRoleService _userRoleService;
        private readonly WebApplication1.Services.Interfaces.IAuthorizationService _authorizationService;

        public UserRoleController(
            ILogger<UserRoleController> logger, 
            IUserRoleService userRoleService, 
            Services.Interfaces.IAuthorizationService authorizationService)
        {
            _logger = logger;
            _userRoleService = userRoleService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll(CancellationToken cancel)
        {
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "read", cancel))
                {
                    return Ok(await _userRoleService.FindAllAsync(cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> FindOneByUserId([FromRoute] int userId, CancellationToken cancel)
        {
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "read", cancel))
                {
                    return Ok(await _userRoleService.FindOneByUserIdAsync(userId, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"User with ID {userId} or Role Not Found, Message: {ex.Message}");
                return NotFound($"User with ID {userId} or Role Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRoleDto updateUserRoleDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"UpdateUserRoleDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "update", cancel))
                {
                    return Ok(await _userRoleService.UpdateAsync(updateUserRoleDto, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"User with ID {updateUserRoleDto.UserId} Not Found, Message: {ex.Message}");
                return NotFound($"User with ID {updateUserRoleDto.UserId} Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
