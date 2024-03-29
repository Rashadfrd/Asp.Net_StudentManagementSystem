﻿using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Models;
using Asp.NetStudentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetStudentManagementSystem.Controllers
{
    [Authorize(Roles="Member")]
    public class StudentJournalController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentJournalController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentStudent = _context.UserInfos.FirstOrDefault(x => x.Email == currentUser.Email);
            StudentJournalVM vm = new StudentJournalVM();
            vm.GroupSubjects = _context.GroupSubjects.Include(x => x.Subject).Where(x => x.GroupId == currentStudent.GroupId);
            if (id is null)
            {
                return View(vm);
            }
            vm.Attendances = _context.Attendances.Include(x => x.Student).Where(x => x.SubjectId == id && x.StudentId == currentStudent.Id);
            return View(vm);
        }

        public async Task<IActionResult> Exam()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentStudent = _context.UserInfos.FirstOrDefault(x => x.Email == currentUser.Email);
            var exam = _context.Exams.Include(x => x.Subject).Include(x => x.Group).Where(x => x.GroupId == currentStudent.GroupId && x.isDeleted == false);
            return View(exam);
        }

        public async Task<IActionResult> Schedule()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentStudent = _context.UserInfos.FirstOrDefault(x => x.Email == currentUser.Email);
            var exam = _context.Schedules.Include(x => x.Subject)
                .Include(x => x.Group)
                .Include(x => x.LessonDay)
                .Include(x => x.LessonHour)
                .Where(x => x.GroupId == currentStudent.GroupId && x.isDeleted == false);
            return View(exam);
        }

        public async Task<IActionResult> Appeal()
        {
            var currentStudent = await _userManager.GetUserAsync(User);
            var studentAppeals = _context.StudentAppeals.Include(x => x.AppUser).Include(x => x.Subject).Where(x=> x.AppUserId == currentStudent.Id && x.IsAllowed == true);
            return View(studentAppeals);
        }

        public async Task<IActionResult> AppealCreate()
        {
            ViewBag.IsSent = false;
            ViewBag.Subjects = _context.Subjects.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AppealCreate(StudentAppeal appeal)
        {

            ViewBag.Subjects = _context.Subjects.ToList();
            var currentUser = await _userManager.GetUserAsync(User);

            appeal.Date = DateTime.Now;
            appeal.AppUserId = currentUser.Id;
            appeal.IsActive = true;
            appeal.IsAllowed = false;
            if (!ModelState.IsValid) return View();
            ViewBag.IsSent = true;

            _context.StudentAppeals.Add(appeal);
            _context.SaveChanges();
            return View();
        }
    }
}
