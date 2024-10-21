using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolePermissionController : Controller
    {
        private readonly ILogger<RolePermissionController> _logger;
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionController(ILogger<RolePermissionController> logger, IRolePermissionService rolePermissionService)
        {
            _logger = logger;
            _rolePermissionService = rolePermissionService;
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
                return Ok(await _rolePermissionService.CreateAsync(createRolePermissionDto, cancel));
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
                return Ok(await _rolePermissionService.FindAllAsync(cancel));
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
                return Ok(await _rolePermissionService.RemoveAsync(roleId, permissionId, cancel));
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
