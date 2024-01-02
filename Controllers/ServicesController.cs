using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Controllers
{
    public class ServicesController : BaseController
    {
        public ServicesController(AppDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.count = await _context.Services.Where(x => !x.IsDeleted).CountAsync();
            return View();
        }
        public async Task<IActionResult> LoadMore(int skip=0)
        {
            //List<Service> services = await _context.Services
            //                            .Where(s => !s.IsDeleted)
            //                            .OrderByDescending(s => s.Id)
            //                            .Skip(skip)
            //                            .Take(8)
            //                            .Include(s => s.ServiceImages)
            //                            .ToListAsync();
            //return PartialView("_ServicesPartialView", services);
            return ViewComponent("Service", new {skip=skip});
        }
    }
}
