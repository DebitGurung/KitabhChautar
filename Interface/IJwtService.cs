using Microsoft.AspNetCore.Identity;

namespace kitabhChautari.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(IdentityUser user, string? role);
    }
}