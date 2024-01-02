using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using WebFrontToBack.DAL;
using WebFrontToBack.ViewModel;

namespace WebFrontToBack.Controllers
{
    public class BasketController : Controller
    {
        private const string COOKIES_BASKET = "basketVM";
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<BasketItemVM> basketItemVMs = new List<BasketItemVM>();
            List<BasketVM> basketVMs;
            if (Request.Cookies[COOKIES_BASKET] != null)
            {
                basketVMs=JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET]);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }
            foreach (var item in basketVMs)
            {
                BasketItemVM basketItemVM = _context.Services
                                                    .Where(s => !s.IsDeleted && s.Id == item.ServiceId)
                                                    .Include(s => s.Category)
                                                    .Include(s => s.ServiceImages)
                                                    .Select(s => new BasketItemVM
                                                    {
                                                        Name = s.Name,
                                                        Id = s.Id,
                                                        CategoryName = s.Category.Name,
                                                        IsDeleted = s.IsDeleted,
                                                        Price = s.Price,
                                                        ServiceCount = item.Count,
                                                        ImagePath = s.ServiceImages.FirstOrDefault(i=>i.IsActive).Path
                                                    }).FirstOrDefault();
                basketItemVMs.Add(basketItemVM);
            }
            return View(basketItemVMs);
        }
        public IActionResult AddBasket(int id,string? ReturnUrl)
        {


            List<BasketVM> basketVMList;
            if (Request.Cookies[COOKIES_BASKET] != null)
            {
                basketVMList = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET]);
            }
            else
            {
                basketVMList = new List<BasketVM>();
            }

            BasketVM oldbasket = basketVMList.Where(x => x.ServiceId == id).FirstOrDefault();
            if (oldbasket!=null)
            {
                oldbasket.Count++;
            }
            else
            {
                BasketVM basketVM = new BasketVM()
                {
                    ServiceId = id,
                    Count = 1
                };
                basketVMList.Add(basketVM);
            }
            Response.Cookies.Append(COOKIES_BASKET,JsonConvert.SerializeObject(basketVMList),new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(30)
            });
            if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl != "/Services/LoadMore")
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Services");
        }
    }
}
 