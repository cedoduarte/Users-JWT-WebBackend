using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
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
                return Ok(await _userService.CreateAsync(createUserDto, cancel));
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
                return Ok(await _userService.FindAllAsync(cancel));
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
                return Ok(await _userService.FindOneAsync(id, cancel));
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
                return Ok(await _userService.UpdateAsync(updateUserDto, cancel));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"User with username '{updateUserDto.Username}' Not Found, Message: {ex.Message}");
                return NotFound($"User with username '{updateUserDto.Username}' Not Found");
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
                return Ok(await _userService.SoftDeleteAsync(id, cancel));
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
