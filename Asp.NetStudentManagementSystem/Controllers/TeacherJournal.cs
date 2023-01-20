using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Models;
using Asp.NetStudentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetStudentManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeacherJournal : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeacherJournal(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Journal()
        {
            HomeVM vm = new HomeVM();
            var currentUser = await _userManager.GetUserAsync(User);
            Teacher currentTeacher = _context.Teachers.FirstOrDefault(x => x.Email == currentUser.Email);

            vm.GroupSubjects = _context.GroupSubjects.Include(x => x.Group).Where(x => x.SubjectId == currentTeacher.SubjectId).ToList();
            return View(vm);
        }

        public async Task<IActionResult> UpdateJournal()
        {
            HomeVM vm = new HomeVM();
            var currentUser = await _userManager.GetUserAsync(User);
            Teacher currentTeacher = _context.Teachers.FirstOrDefault(x => x.Email == currentUser.Email);

            vm.GroupSubjects = _context.GroupSubjects.Include(x => x.Group).Where(x => x.SubjectId == currentTeacher.SubjectId).ToList();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetUpdateJournal(int GroupId, DateTime Date)
        {
            HomeVM vm = new HomeVM();
            var currentUser = await _userManager.GetUserAsync(User);
            Teacher currentTeacher = _context.Teachers.FirstOrDefault(x => x.Email == currentUser.Email);

            vm.Attendance = _context.Attendances.Include(x => x.Student)
                                 .Where(x => x.SubjectId == currentTeacher.SubjectId && x.Date == Date && x.Student.GroupId == GroupId).ToList();
            ViewBag.SubjectId = currentTeacher.SubjectId;

            return PartialView("_UpdateJournalPartialView", vm);
        }


        [HttpPost]
        public async Task<IActionResult> GetJournal(int GroupId, DateTime Date)
        {
            HomeVM vm = new HomeVM();
            var currentUser = await _userManager.GetUserAsync(User);
            Teacher currentTeacher = _context.Teachers.FirstOrDefault(x => x.Email == currentUser.Email);

            vm.GroupSubjects = _context.GroupSubjects.Include(x => x.Group).Where(x => x.SubjectId == currentTeacher.SubjectId).ToList();


            vm.CurrentGroup = _context.Groups
                                                  .Include(x => x.Students)
                                                  .ThenInclude(x => x.Attendances)
                                                  .Include(x => x.GroupSubjects)
                                                  .ThenInclude(x => x.Subject)
                                                  .FirstOrDefault(x => x.Id == GroupId);
            vm.Subject = _context.Subjects.Include(x => x.GroupSubjects)
                                            .ThenInclude(x => x.Group)
                                            .ThenInclude(x => x.Students)
                                            .FirstOrDefault(x => x.Id == currentTeacher.SubjectId);

            //vm.CurrentStudent = _context.UserInfos.Include(x=>x.Attendances).ThenInclude(x=>x.Subject).FirstOrDefault(x => x.Email == currentUser.Email);

            vm.Attendance = _context.Attendances.Include(x => x.Student).Include(x => x.Subject).Where(x => x.Date == Date && x.SubjectId == currentTeacher.SubjectId && x.Student.GroupId == GroupId).ToList();
            vm.Date = Date;

            return PartialView("_JournalPartialView", vm);
        }

        [HttpPost]
        public IActionResult Form(ICollection<Attendance> attendances)
        {
            foreach (var item in attendances)
            {
                if (item.GradeOrAbsent == -1)
                {
                    _context.Attendances.Add(new() { StudentId = item.StudentId, SubjectId = item.SubjectId, IsPresent = false, Date = item.Date, Grade = item.Grade });
                }
                else if (item.GradeOrAbsent == -2)
                {

                    _context.Attendances.Add(new() { StudentId = item.StudentId, SubjectId = item.SubjectId, IsPresent = true, Date = item.Date, Grade = item.Grade });
                }
                else
                {
                    _context.Attendances.Add(new() { StudentId = item.StudentId, SubjectId = item.SubjectId, IsPresent = true, Date = item.Date, Grade = (byte)item.GradeOrAbsent });
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Journal));
        }

        [HttpPost]
        public IActionResult Update(ICollection<Attendance> attendances)
        {
            foreach (var item in attendances)
            {
                if (item.GradeOrAbsent == -1)
                {
                    Attendance att = _context.Attendances.Find(item.Id);
                    att.IsPresent = false;
                    att.Grade = null;
                }
                else if (item.GradeOrAbsent == -2)
                {
                    Attendance att = _context.Attendances.Find(item.Id);
                    att.IsPresent = true;
                }
                else
                {
                    Attendance att = _context.Attendances.Find(item.Id);
                    att.IsPresent = true;
                    att.Grade = (byte)item.GradeOrAbsent;
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(UpdateJournal));
        }
    }
}
