using AuthService.Application.DTOs.login;
using AuthService.Application.DTOs.register;
using AuthService.Application.Interfaces;
using AuthService.Domain;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services
{
    public class UserService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Result<LoginResponseDto>.Fail("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) return Result<LoginResponseDto>.Fail("Invalid credentials");

            var role = await _userManager.GetRolesAsync(user);
            var userRole = role.FirstOrDefault() ?? "USER";

            var token = _tokenService.CreateToken(user, Enum.Parse<RoleUser>(userRole));

            var response = new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Role = userRole,
                IsFirstLogin = user.IsFirstLogin
            };

            return Result<LoginResponseDto>.Ok(response);
        }

        public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterDto request)
        {
            var checkUser = await _userManager.FindByEmailAsync(request.Email);
            if (checkUser != null)
                return Result<RegisterResponseDto>.Fail("Email already in use");

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                IsFirstLogin = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Result<RegisterResponseDto>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, request.Role);

            var response = new RegisterResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Role = request.Role
            };

            return Result<RegisterResponseDto>.Ok(response);
        }

        public async Task<string> Logout()
        {
            await _signInManager.SignOutAsync();
            return "User logged out successfully";
        }
    }
}
