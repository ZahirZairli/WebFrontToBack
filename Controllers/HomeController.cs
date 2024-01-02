using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.ViewModel;

namespace WebFrontToBack.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        HttpContext.Session.SetString("Name", "Zahir"); //To set a data to session
        Response.Cookies.Append("Surname", "Zairli", new CookieOptions
        {
            //MaxAge = TimeSpan.FromSeconds(10),
            Expires = DateTimeOffset.Now.AddSeconds(10)
        }); //To set a data to cookie
        HomeIndex homeIndex = new HomeIndex()
        {
            Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync(),
        };
        return View(homeIndex);
    }
    public IActionResult GetSessionAndCookie()
    {
        string name = HttpContext.Session.GetString("Name"); //Get the data from session
        string surname = Request.Cookies["Surname"]; //Get the data from cookie
        return Json($"{name} {surname}");
    }
}
