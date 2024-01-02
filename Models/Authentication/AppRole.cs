using Microsoft.AspNetCore.Identity;

namespace WebFrontToBack.Models.Authentication
{
    public class AppRole:IdentityRole
    {
        public bool isActivated { get; set; }
    }
}
