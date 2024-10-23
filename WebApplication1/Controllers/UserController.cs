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
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly WebApplication1.Services.Interfaces.IAuthorizationService _authorizationService;        

        public UserController(
            ILogger<UserController> logger, 
            IUserService userService,
            WebApplication1.Services.Interfaces.IAuthorizationService authorizationService)
        {
            _logger = logger;
            _userService = userService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto, CancellationToken cancel)
        {      
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"CreateUserDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "create", cancel))
                {
                    return Ok(await _userService.CreateAsync(createUserDto, cancel));
                }
                else
                {
                    return Forbid();
                }
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
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
                    return Ok(await _userService.FindAllAsync(cancel));
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
                    return Ok(await _userService.FindOneAsync(id, cancel));
                }
                else
                {
                    return Forbid();
                }                
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"User with ID {id} Not Found, Message: {ex.Message}");
                return NotFound($"User with ID {id} Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"UpdateUserDto has validation errors: {ModelState}");
                return BadRequest(ModelState);
            }
            try
            {
                if (await _authorizationService.HasPermissionAsync(User.FindFirst(ClaimTypes.Role)?.Value, "update", cancel))
                {
                    return Ok(await _userService.UpdateAsync(updateUserDto, cancel));
                }
                else
                {
                    return Forbid();
                }
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"User with ID '{updateUserDto.Id}' Not Found, Message: {ex.Message}");
                return NotFound(ex.Message);
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
                    return Ok(await _userService.SoftDeleteAsync(id, cancel));
                }
                else
                {
                    return Forbid();
                }
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"User with ID {id} Not Found, Message: {ex.Message}");
                return NotFound($"User with ID {id} Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexptected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
