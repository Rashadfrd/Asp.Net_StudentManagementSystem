using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Enums;
using Asp.NetStudentManagementSystem.Models;
using Asp.NetStudentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetStudentManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class TeacherController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeacherController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var teachers = _context.Teachers.Include(x => x.Subject);
            return View(teachers);
        }

        public IActionResult Create()
        {
            ViewBag.Subjects = _context.Subjects.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRegistrationVM teacher)
        {
            if (!ModelState.IsValid) return View();
            var existUser = await _userManager.FindByEmailAsync(teacher.Email);
            if (existUser != null)
            {
                ModelState.AddModelError("Email", "Bu email vasitəsilə hesab artıq mövcuddur.");
                return View();
            }
            Teacher newTeacher = new()
            {
                Name = teacher.Name,
                Surname = teacher.Surname,
                isActive = true,
                Email = teacher.Email,
                DateOfBirth = teacher.DateOfBirth,
                SubjectId = teacher.SubjectId
            };
            await _context.Teachers.AddAsync(newTeacher);
            await _context.SaveChangesAsync();
            AppUser userCreate = new()
            {
                Name = teacher.Name,
                Surname = teacher.Surname,
                Email = teacher.Email,
                UserName = teacher.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(userCreate, teacher.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.ToString());
                }
                return View();
            }
            await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync(userCreate.Email), Roles.Admin.ToString());
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            ViewBag.Subjects = _context.Subjects.ToList();
            var teacher = _context.Teachers.Find(id);
            return View(teacher);
        }


        [HttpPost]
        public IActionResult Update(int? id, Teacher teacher)
        {
            ViewBag.Subjects = _context.Subjects.ToList();
            if (id != teacher.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(teacher);
            var editTeacher = _context.Teachers.Find(id);
            if (editTeacher is null) return NotFound();
            editTeacher.Name = teacher.Name;
            editTeacher.Surname = teacher.Surname;
            editTeacher.DateOfBirth = teacher.DateOfBirth;
            editTeacher.Email = teacher.Email;
            editTeacher.SubjectId = teacher.SubjectId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
