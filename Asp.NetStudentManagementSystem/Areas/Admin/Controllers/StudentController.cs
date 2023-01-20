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
    public class StudentController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var students = _context.UserInfos.Include(x => x.Group);
            return View(students);
        }
        public IActionResult Create()
        {
            ViewBag.Groups = _context.Groups.ToList();
            return View();  
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserRegistrationVM student)
        {
            if (!ModelState.IsValid) return View();
            var existUser = await _userManager.FindByEmailAsync(student.Email);
            if (existUser != null)
            {
                ModelState.AddModelError("Email", "Bu email vasitəsilə hesab artıq mövcuddur.");
                return View();
            }
            UsersInfo userInfo = new()
            {
                Name = student.Name,
                Surname = student.Surname,
                Gender = student.Gender,
                isActive = true,
                Email = student.Email,
                Country = student.Country,
                FatherName = student.FatherName,
                DateOfBirth = student.DateOfBirth,
                GroupId = student.GroupId
            };
            await _context.UserInfos.AddAsync(userInfo);
            await _context.SaveChangesAsync();
            AppUser userCreate = new()
            {
                Name = student.Name,
                Surname = student.Surname,
                Email = student.Email,
                UserName = student.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(userCreate, student.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.ToString());
                }
                return View();
            }
            await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync(userCreate.Email), Roles.SuperAdmin.ToString());
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            ViewBag.GroupsUpdate = _context.Groups.ToList();
            var usersInfo = _context.UserInfos.Find(id);
            return View(usersInfo);
        }


        [HttpPost]

        public IActionResult Update(int? id, UsersInfo user)
        {
            ViewBag.GroupsUpdate = _context.Groups.ToList();
            if (id != user.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(user);
            var editStudent = _context.UserInfos.Find(id);
            if (editStudent is null) return NotFound();
            editStudent.Name = user.Name;
            editStudent.Surname = user.Surname;
            editStudent.FatherName = user.FatherName;
            editStudent.DateOfBirth = user.DateOfBirth;
            editStudent.Email = user.Email;
            editStudent.Country = user.Country;
            editStudent.Gender = user.Gender;
            editStudent.GroupId = user.GroupId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
