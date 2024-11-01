using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services.Interfaces;
using WebApplication1.Shared;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;
        private readonly WebApplication1.Services.Interfaces.IAuthorizationService _authorizationService;

        public RoleController(
            ILogger<RoleController> logger, 
            IRoleService roleService, 
            Services.Interfaces.IAuthorizationService authorizationService)
        {
            _logger = logger;
            _roleService = roleService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto createRoleDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"CreateRoleDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                if (await _authorizationService.HasPermissionAsync(
                    User.FindFirst(Constants.Jwt.UserIdClaim)!.Value,
                    Constants.Permissions.Create,
                    cancel))
                {
                    var createdRole = await _roleService.CreateAsync(createRoleDto, cancel);
                    return CreatedAtAction(nameof(FindOne), new { id = createdRole.Id }, createdRole);
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
                if (await _authorizationService.HasPermissionAsync(
                    User.FindFirst(Constants.Jwt.UserIdClaim)!.Value,
                    Constants.Permissions.Read, 
                    cancel))
                {
                    return Ok(await _roleService.FindAllAsync(cancel));
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
                if (await _authorizationService.HasPermissionAsync(
                    User.FindFirst(Constants.Jwt.UserIdClaim)!.Value,
                    Constants.Permissions.Read,
                    cancel))
                {
                    return Ok(await _roleService.FindOneAsync(id, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Role with ID {id} Not Found, Message: {ex.Message}");
                return NotFound($"Role with ID {id} Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoleDto updateRoleDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"UpdateRoleDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                if (await _authorizationService.HasPermissionAsync(
                    User.FindFirst(Constants.Jwt.UserIdClaim)!.Value,
                    Constants.Permissions.Update,
                    cancel))
                {
                    return Ok(await _roleService.UpdateAsync(updateRoleDto, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Role with ID '{updateRoleDto.Id}' Not Found, Message: {ex.Message}");
                return NotFound($"Role with ID '{updateRoleDto.Id}' Not Found");
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
                if (await _authorizationService.HasPermissionAsync(
                    User.FindFirst(Constants.Jwt.UserIdClaim)!.Value,
                    Constants.Permissions.Delete, 
                    cancel))
                {
                    return Ok(await _roleService.SoftDeleteAsync(id, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Role with ID {id} Not Found, Message: {ex.Message}");
                return NotFound($"Role with ID {id} Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
