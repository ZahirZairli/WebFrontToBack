using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using WebFrontToBack.Utilities.Extensions;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeamMemberController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webhostenvironment; //Bundan biz projenin locaationlarini tuta bilerik
        public TeamMemberController(AppDbContext context, IWebHostEnvironment webhostenvironment)
        {
            _context = context;
            _webhostenvironment = webhostenvironment;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<TeamMember> members = await _context.TeamMembers.ToListAsync();
            return View(members);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamMemberVM team)
        {
            if (!ModelState.IsValid)
            {
                return View(team);
            }
            if (!team.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"Pleas select an image file!Your file:{team.Photo.FileName}");
                return View(); 
            }
            if (!team.Photo.CheckContentSize(200))
            {
                ModelState.AddModelError("Photo", $"Please select an image lesser than 200kb!Your file's size:{team.Photo.Length / 1024}Kb");
                return View();
            }
            string root =Path.Combine(_webhostenvironment.WebRootPath,"assets","img");  //WebRootPath ile biz wwwroot un yerin tuturuq
            string fileName = await team.Photo.SaveAsync(root);

            TeamMember teamMember = new TeamMember()
            {
                Fullname = team.Fullname,
                Imagepath = fileName,
                Profession = team.Profession,
            };
            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            TeamMember member = await _context.TeamMembers.FindAsync(id);
            if (member != null)
            {
                string photoPath = Path.Combine
                    (_webhostenvironment.WebRootPath,"assets","img",member.Imagepath);
                if (System.IO.File.Exists(photoPath))
                {
                    System.IO.File.Delete(photoPath);
                }
                _context.TeamMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            TeamMember member = await _context.TeamMembers.FindAsync(id);
            if (member != null)
            {
                return View(member);
            }
            TempData["teamexist"] = "Team member does not exist!";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TeamMember member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            TeamMember? old = await _context.TeamMembers.FindAsync(member.Id);
            if (old != null)
            {
                old.Fullname = member.Fullname;
                old.Profession = member.Profession;
                old.Imagepath = member.Imagepath;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Content("There is not any data!");
        }
    }
}
