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
    public class ExamController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExamController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var exams = _context.Exams.Include(x=>x.Group).Include(x => x.Subject);
            return View(exams);
        }

        public IActionResult Create()
        {
            ViewBag.Groups = _context.Groups.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Exam exam)
        {
            if (!ModelState.IsValid) return View();
            _context.Exams.Add(exam);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            ViewBag.Groups = _context.Groups.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            var exam = _context.Exams.Find(id);
            return View(exam);
        }

        [HttpPost]
        public IActionResult Update(int? id, Exam exam)
        {
            ViewBag.Groups = _context.Groups.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            if (id != exam.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(exam);
            var editExam = _context.Exams.Find(id);
            if (editExam is null) return BadRequest();
            editExam.Date = exam.Date;
            editExam.SubjectId = exam.SubjectId;
            editExam.GroupId = exam.GroupId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
