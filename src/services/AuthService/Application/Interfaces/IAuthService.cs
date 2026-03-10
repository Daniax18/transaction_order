using AuthService.Application.DTOs.login;
using AuthService.Application.DTOs.register;
using AuthService.Application.DTOs.users;

namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<RegisterResponseDto>> RegisterAsync(RegisterDto request);
        Task<Result<LoginResponseDto>> LoginAsync(LoginDto request);
        Task<string> Logout();

        Task<Result<List<UsersDto>>> GetUsersAsync();
    }
}
