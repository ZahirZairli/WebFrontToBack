using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Models;
using WebFrontToBack.Models.Authentication;

namespace WebFrontToBack.DAL;

//public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ServiceImage> ServiceImages { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<Setting> Settings { get; set; }
}
