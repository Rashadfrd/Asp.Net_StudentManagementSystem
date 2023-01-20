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
    public class ScheduleController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ScheduleController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var schedules = _context.Schedules.Include(x => x.LessonHour).Include(x => x.LessonDay).Include(x => x.Group).Include(x => x.Subject);
            return View(schedules);
        }
        public IActionResult Create()
        {
            ViewBag.Time = _context.LessonHours.ToList();
            ViewBag.Days = _context.LessonDays.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            ViewBag.Groups = _context.Groups.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Schedule schedule)
        {
            if (!ModelState.IsValid) return View();
            _context.Schedules.Add(schedule);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            ViewBag.Time = _context.LessonHours.ToList();
            ViewBag.Days = _context.LessonDays.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            ViewBag.Groups = _context.Groups.ToList();

            var schedule = _context.Schedules.Find(id);
            return View(schedule);
        }

        [HttpPost]
        public IActionResult Update(int? id, Schedule schedule)
        {
            ViewBag.Time = _context.LessonHours.ToList();
            ViewBag.Days = _context.LessonDays.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            ViewBag.Groups = _context.Groups.ToList();
            if (id != schedule.Id || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(schedule);
            var editSchedule = _context.Schedules.Find(id);
            if (editSchedule is null) return BadRequest();
            editSchedule.Room = schedule.Room;
            editSchedule.LessonDayId = schedule.LessonDayId;
            editSchedule.LessonHourId = schedule.LessonHourId;
            editSchedule.SubjectId = schedule.SubjectId;
            editSchedule.GroupId = schedule.GroupId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
