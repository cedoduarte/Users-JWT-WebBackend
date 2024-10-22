using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : Controller
    {
        private readonly ILogger<PermissionController> _logger;
        private readonly IPermissionService _permissionService;

        public PermissionController(ILogger<PermissionController> logger, IPermissionService permissionService)
        {
            _logger = logger;
            _permissionService = permissionService;
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
                return Ok(await _permissionService.CreateAsync(createPermissionDto, cancel));
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
                return Ok(await _permissionService.FindAllAsync(cancel));
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
                return Ok(await _permissionService.FindOneAsync(id, cancel));
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
                return Ok(await _permissionService.UpdateAsync(updatePermissionDto, cancel));
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
                return Ok(await _permissionService.SoftDeleteAsync(id, cancel));
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
