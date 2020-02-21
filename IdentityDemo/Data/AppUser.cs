using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Data
{
    public class AppUser : IdentityUser
    {
        public int Sex { get; set; }
    }
}
