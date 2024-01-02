using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using WebFrontToBack.Utilities.Constants;
using WebFrontToBack.Utilities.Extensions;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    { 
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<Category> _categories;

        public ServiceController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _categories = _context.Categories.Where(x => !x.IsDeleted).ToList();
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Service> services = await _context.Services
                                           .Where(x=>!x.IsDeleted)
                                           .OrderByDescending(x=>x.Id)
                                           .Take(8)
                                           .Include(x=>x.Category)
                                           .Include(x => x.ServiceImages)
                                           .ToListAsync();
            return View(services);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateServiceVM service = new CreateServiceVM()
            {
                Categories = _categories
            };
            return View(service);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceVM createServiceVm)
        {
            createServiceVm.Categories = _categories;
            if (!ModelState.IsValid)
            {
                return View(createServiceVm);
            }
            foreach (var item in createServiceVm.Photos)
            {
                if (!item.CheckContentType("image/"))
                {
                    ModelState.AddModelError("Photos",$"Your file:{item.FileName},{Messages.FileTypeMustBeImage}");
                    return View(createServiceVm);
                }
                if (!item.CheckContentSize(200))
                {
                    ModelState.AddModelError("Photos",$"{Messages.FileSizeMustBe200kb} Your File {item.FileName} and it's size {item.Length/1024}kb");
                    return View(createServiceVm);
                }
            }
            List<ServiceImage> images = new List<ServiceImage>();
            string rootpath = Path.Combine(_webHostEnvironment.WebRootPath,"assets","img");
            foreach (var item in createServiceVm.Photos)
            {
                string fileName = await item.SaveAsync(rootpath);
                ServiceImage serviceImage = new ServiceImage()
                {
                    Path = fileName
                };
                if (!images.Any(i=>i.IsActive))
                {
                    serviceImage.IsActive = true;
                }
                images.Add(serviceImage);
            }
            Service newService = new Service()
            {
                CategoryId = createServiceVm.CategoryId,
                ServiceImages = images,
                Name = createServiceVm.Name,
                Description = createServiceVm.Description,
                Price = createServiceVm.Price,
            }; 
            await _context.Services.AddAsync(newService);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
