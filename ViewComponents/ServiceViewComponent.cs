using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.ViewComponents
{
    public class ServiceViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public ServiceViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int skip=0)
        {
            List<Service> services = await _context.Services
                                                   .Where(x => !x.IsDeleted)
                                                   .OrderByDescending(x => x.Id)
                                                   .Skip(skip)
                                                   .Take(8)
                                                   .Include(x => x.Category)
                                                   .Include(x => x.ServiceImages)
                                                   .ToListAsync();
            return View(services);
        }
    }
}
