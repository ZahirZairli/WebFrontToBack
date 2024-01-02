using Microsoft.AspNetCore.Mvc;
using WebFrontToBack.DAL;

namespace WebFrontToBack.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AppDbContext _context;

        public BaseController(AppDbContext context)
        {
            _context = context;
        }
    }
}
