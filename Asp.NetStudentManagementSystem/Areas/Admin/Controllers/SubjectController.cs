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
    public class SubjectController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SubjectController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var subject = _context.Subjects;
            return View(subject);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Subject subject)
        {
            if(!ModelState.IsValid) return View();
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var subject = _context.Subjects.Find(id);
            return View(subject);
        }

        [HttpPost]
        public IActionResult Update(int? id, Subject subject)
        {
            if(id != subject.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(subject);
            var editSubject = _context.Subjects.Find(id);
            if (editSubject is null) return BadRequest();
            editSubject.Name = subject.Name;
            editSubject.Hours = subject.Hours;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
