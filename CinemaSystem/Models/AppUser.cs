using Microsoft.AspNetCore.Identity;

namespace CinemaSystem.Models
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
