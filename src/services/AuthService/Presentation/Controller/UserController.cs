using AuthService.Application.DTOs.login;
using AuthService.Application.DTOs.register;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _authService.LoginAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);

        }

        [HttpGet("users")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _authService.GetUsersAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);
        }

        [HttpGet("others")]
        public async Task<IActionResult> GetOthersUserById([FromQuery] string userId)
        {
            var result = await _authService.GetOtherUsersById(userId);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Value);
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetUserNamesByIds([FromQuery(Name = "ids")] string[] ids)
        {
            if (ids == null || ids.Length == 0)
                return BadRequest("Aucun id fourni.");

            var result = await _authService.GetUserNamesByIds(ids);

            if (!result.IsSuccess)
                return StatusCode(500, result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
