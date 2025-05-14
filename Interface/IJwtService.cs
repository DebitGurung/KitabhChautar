using Microsoft.AspNetCore.Identity;

namespace kitabhChauta.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(IdentityUser user, string? role);
    }
}