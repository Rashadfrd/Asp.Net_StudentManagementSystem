using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetStudentManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class AppealController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AppealController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var appeals = _context.StudentAppeals.Include(x => x.AppUser).Include(x => x.Subject);
            return View(appeals);
        }

        public IActionResult Update(int id)
        {
            var studentAppeal = _context.StudentAppeals.Include(x=>x.AppUser).Include(x => x.Subject).FirstOrDefault(x=>x.Id == id);
            return View(studentAppeal);
        }


        [HttpPost]
        public IActionResult Update(int? id, StudentAppeal appeal )
        {
            if (id != appeal.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(appeal);
            var editAppeal = _context.StudentAppeals.Find(id);
            if (editAppeal is null) return BadRequest();
            editAppeal.IsAllowed = appeal.IsAllowed;
            editAppeal.IsActive = appeal.IsActive;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
