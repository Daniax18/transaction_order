using AuthService.Domain;

namespace AuthService.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, RoleUser role);
    }
}
