using Microsoft.AspNetCore.Identity;

namespace WebFrontToBack.Models.Authentication
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; }
        public bool isActivated { get; set; }
    }
}
