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
    public class PermissionController : Controller
    {
        private readonly ILogger<PermissionController> _logger;
        private readonly IPermissionService _permissionService;
        private readonly WebApplication1.Services.Interfaces.IAuthorizationService _authorizationService;

        public PermissionController(
            ILogger<PermissionController> logger, 
            IPermissionService permissionService, 
            WebApplication1.Services.Interfaces.IAuthorizationService authorizationService)
        {
            _logger = logger;
            _permissionService = permissionService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionDto createPermissionDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"CreatePermissionDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "create", cancel))
                {
                    return Ok(await _permissionService.CreateAsync(createPermissionDto, cancel));
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

        [HttpGet]
        public async Task<IActionResult> FindAll(CancellationToken cancel)
        {
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "read", cancel))
                {
                    return Ok(await _permissionService.FindAllAsync(cancel));
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

        [HttpGet("{id}")]
        public async Task<IActionResult> FindOne([FromRoute] int id, CancellationToken cancel)
        {
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "read", cancel))
                {
                    return Ok(await _permissionService.FindOneAsync(id, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Permission with ID {id} Not Found, Message: {ex.Message}");
                return NotFound($"Permission with ID {id} Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePermissionDto updatePermissionDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"UpdatePermissionDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "update", cancel))
                {
                    return Ok(await _permissionService.UpdateAsync(updatePermissionDto, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Permission with ID '{updatePermissionDto.Id}' Not Found, Message: {ex.Message}");
                return NotFound($"Permission with ID '{updatePermissionDto.Id}' Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id, CancellationToken cancel)
        {
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "delete", cancel))
                {
                    return Ok(await _permissionService.SoftDeleteAsync(id, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Permission with ID {id} Not Found, Message: {ex.Message}");
                return NotFound($"Permission with ID {id} Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
