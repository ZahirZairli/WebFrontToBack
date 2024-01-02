using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebFrontToBack.Models.Authentication;
using WebFrontToBack.ViewModel.Authentication;

namespace WebFrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser newUser = new AppUser()
            {
                Fullname = registerVM.Fullname,
                UserName = registerVM.Username,
                Email = registerVM.Email
            };
            IdentityResult registerResult = await _userManager.CreateAsync(newUser,registerVM.Password);
            if (!registerResult.Succeeded)
            {
                foreach (var item in registerResult.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                return View(registerVM);
            }
            IdentityResult roleResult =  await _userManager.AddToRoleAsync(newUser,UserRoles.User.ToString());
            if (!roleResult.Succeeded)
            {
                foreach (var item in roleResult.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                return View(registerVM);
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM,string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or password is wrong!");
                return View(loginVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signinManager.CheckPasswordSignInAsync(appUser,loginVM.Password,true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account was blocked for 3 minutes");
                return View(loginVM);
            }
            if (!signInResult.Succeeded)
            {
                    ModelState.AddModelError("", "Email or password is wrong!");
                    return View(loginVM);
            }
            await _signinManager.SignInAsync(appUser,loginVM.RememberMe);
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout(string? ReturnUrl)
        {
            await _signinManager.SignOutAsync();
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
        //public async Task<IActionResult> AddRole()
        //{
        //    foreach (var item in Enum.GetValues(typeof(UserRoles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(item.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString()});
        //        }
        //    }

        //    return Json("Ok");
        //}
        enum UserRoles
        {
            Admin,
            User,
            Moderator
        }
    }
}
  