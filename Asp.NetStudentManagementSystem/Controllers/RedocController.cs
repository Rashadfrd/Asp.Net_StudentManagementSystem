using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Models;
using Asp.NetStudentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetStudentManagementSystem.Controllers
{
    [Authorize]
    public class RedocController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        public RedocController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> FormAppeal()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            RedocVM vm = new RedocVM();
            vm.Documents = _context.Documents;
            vm.Student = _context.UserInfos.Include(x=>x.Group).ThenInclude(x=>x.Department).FirstOrDefault(x=>x.Email == currentUser.Email);

            return View(vm);
        }

        [HttpPost]
        public IActionResult FormAppeal(FormAppealVM fa)
        {
            _context.UserDocuments.Add(new()
            {
                DocumentId = fa.DocumentId,
                ToWhere = fa.ToWhere,
                AppUserId = _userManager.GetUserId(User)
            });
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DocumentStatus()
        {
            var currentUserId = _userManager.GetUserId(User);
            var userDocuments = _context.UserDocuments.Include(x=>x.Document).Include(x=>x.AppUser).Where(x => x.AppUserId == currentUserId);
            return View(userDocuments);
        }
    }
}
