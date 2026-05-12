using Microsoft.AspNetCore.Identity;

namespace Transit.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
