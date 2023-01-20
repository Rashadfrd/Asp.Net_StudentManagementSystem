using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetStudentManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class NotificationController : Controller
    {

        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NotificationController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var notifications = _context.Notifications;
            return View(notifications);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Notification notification)
        {
            if (!ModelState.IsValid) return View();
            notification.DateTime = DateTime.Now;
            _context.Notifications.Add(notification);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var notification = _context.Notifications.Find(id);
            return View(notification);
        }

        [HttpPost]
        public IActionResult Update(int? id, Notification notification)
        {
            if (id != notification.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(notification);
            var editNot = _context.Notifications.Find(id);
            if (editNot is null) return BadRequest();
            editNot.Title = notification.Title;
            editNot.Content = notification.Content;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
