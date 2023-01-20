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
    public class GroupController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public GroupController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var groups = _context.Groups.Include(x=>x.Department);
            return View(groups);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Group group)
        {
            if (!ModelState.IsValid) return View();
            _context.Groups.Add(group);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            ViewBag.Departments = _context.Departments.ToList();
            var group = _context.Groups.Find(id);
            return View(group);
        }


        [HttpPost]
        public IActionResult Update(int? id, Group group)
        {
            ViewBag.Departments = _context.Departments.ToList();
            if (id != group.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(group);
            var editGroup = _context.Groups.Find(id);
            if (editGroup is null) return BadRequest();
            editGroup.Name = group.Name;
            editGroup.DepartmentId = group.DepartmentId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
