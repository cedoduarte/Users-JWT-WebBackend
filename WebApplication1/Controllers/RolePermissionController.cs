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
    public class RolePermissionController : Controller
    {
        private readonly ILogger<RolePermissionController> _logger;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly WebApplication1.Services.Interfaces.IAuthorizationService _authorizationService;

        public RolePermissionController(
            ILogger<RolePermissionController> logger, 
            IRolePermissionService rolePermissionService, 
            Services.Interfaces.IAuthorizationService authorizationService)
        {
            _logger = logger;
            _rolePermissionService = rolePermissionService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRolePermissionDto createRolePermissionDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"CreateRolePermissionDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "create", cancel))
                {
                    return Ok(await _rolePermissionService.CreateAsync(createRolePermissionDto, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Role with ID {createRolePermissionDto.RoleId} or Permission with ID {createRolePermissionDto.PermissionId} Not Found, Message: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> FindAll(CancellationToken cancel)
        {
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "read", cancel))
                {
                    return Ok(await _rolePermissionService.FindAllAsync(cancel));
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

        [HttpDelete("role/{roleId}/permission/{permissionId}")]
        public async Task<IActionResult> Remove([FromRoute] int roleId, [FromRoute] int permissionId, CancellationToken cancel)
        {
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "delete", cancel))
                {
                    return Ok(await _rolePermissionService.RemoveAsync(roleId, permissionId, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Role or Permission Not Found, Message: {ex.Message}");
                return NotFound($"Role with ID {roleId} or Permission with Id {permissionId} Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
