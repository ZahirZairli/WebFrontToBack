using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> isExistCategory(string name)
        {
            bool isExist = await _context.Categories.Where(x=>!x.IsDeleted).AnyAsync(x => x.Name.ToLower() == name.ToLower());
            return isExist;
        }

        public async Task<IActionResult> Index(int page=1,int take = 2)
        {

            List<Category> categories = await _context.Categories
                                                             .Where(x => !x.IsDeleted)
                                                             .OrderByDescending(x=>x.Id)
                                                             .Skip((page-1)*take)
                                                             .Take(take)
                                                             .Include(x => x.Services)
                                                             .ToListAsync();
            PaginateVM<Category> paginateVM = new PaginateVM<Category>()
            {
                Data = categories,
                CurrentPage = page,
                PageCount = await GetPageCount(take),
                HasNext = page < await GetPageCount(take),
                HasPrevious = page > 1,
                Take = take
            };
            return View(paginateVM);
        }


        private async Task<int> GetPageCount(int take)
        {
            int catcount = await _context.Categories.Where(x => !x.IsDeleted).CountAsync();
            return (int)Math.Ceiling((double)catcount / take);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!await isExistCategory(category.Name))
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("Name", "Category is exist in the database!");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                return View(category);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Category category)
        {
            Category? old = await _context.Categories.FindAsync(category.Id);
            if (!await isExistCategory(category.Name))
            {
                old.Name = category.Name;
                _context.Categories.Update(old);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            return Content("The category was already created!");
        }
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FindAsync(id);
            if (category !=null)
            {
            category.IsDeleted = true;
            _context.Update(category);
            await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int id)
        {
            Category cat = await _context.Categories.FindAsync(id);
            if (cat !=null)
            {
                Category category = await _context.Categories.Include(x => x.Services).ThenInclude(y=>y.ServiceImages).FirstAsync(x=>x.Id == id);
                return View(category);
            }
            return Content("There is not any data!");
        }
    }
}
