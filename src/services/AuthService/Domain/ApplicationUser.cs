using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain
{
    public class ApplicationUser: IdentityUser
    {
        public bool IsFirstLogin { get; set; } = true;
    }
}
