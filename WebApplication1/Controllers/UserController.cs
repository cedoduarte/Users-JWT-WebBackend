using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserDto createUserDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _userService.CreateAsync(createUserDto, cancel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> FindAllAsync(CancellationToken cancel)
        {
            try
            {
                return Ok(await _userService.FindAllAsync(cancel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindOneAsync([FromRoute] int id, CancellationToken cancel)
        {
            try
            {
                return Ok(await _userService.FindOneAsync(id, cancel));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserDto updateUserDto, CancellationToken cancel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _userService.UpdateAsync(updateUserDto, cancel));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync([FromRoute] int id, CancellationToken cancel)
        {
            try
            {
                return Ok(await _userService.SoftDeleteAsync(id, cancel));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
