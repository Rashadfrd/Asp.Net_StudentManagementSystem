using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Models;
using Asp.NetStudentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Asp.NetStudentManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
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

        public IActionResult Modal(int? id)
        {
            if (id is null) return BadRequest();

            var notification = _context.Notifications.Find(id);
            if (notification is null) return NotFound();
            return PartialView("_ModalPartialView", notification);
        }

        public IActionResult Files()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "assets", "Files");
            string[] filepaths = Directory.GetFiles(path);
            List<FileModel> files = new List<FileModel>();
            foreach (var item in filepaths)
            {
                files.Add(new FileModel() { FileName = Path.GetFileName(item) });
            }
            return View(files);
        }

        public FileResult DownloadFile(string fileName)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "assets", "Files",fileName);
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }

        public async Task<IActionResult> PrivateChat(string? email)
        {
            HomeVM vm = new HomeVM();
            vm.CurrentUser = await _userManager.GetUserAsync(User);
            var SuperAdmins = await _userManager.GetUsersInRoleAsync("SuperAdmin");

            ViewBag.receiver = SuperAdmins.FirstOrDefault().Email.ToString();

            vm.UserMessages = _context.UserMessages.Include(x => x.Messages).Where(x => x.SenderId == vm.CurrentUser.Id).ToList();
            var receiverMessages = _context.UserMessages.Include(x => x.Messages).Where(x => x.ReceiverId == vm.CurrentUser.Id).ToList();
            vm.UserMessages.AddRange(receiverMessages);
            vm.AdminMessages = _context.UserMessages.Include(x => x.Messages).Where(x => x.ReceiverId == SuperAdmins.FirstOrDefault().Id);
            vm.AllUsers = _context.AppUsers.ToList();
            if (email != null)
            {
                var filteredSender = await _userManager.FindByEmailAsync(email);
                ViewBag.receiverFiltered = filteredSender.Email.ToString();
                ViewBag.receiverNameFiltered = filteredSender.Name;
                ViewBag.receiverSurnameFiltered = filteredSender.Surname;
                vm.AdminFilteredMessages = _context.UserMessages.Include(x => x.Messages).Where(x => x.SenderId == filteredSender.Id && x.ReceiverId == vm.CurrentUser.Id).ToList();
                var AdminFilteredSendedMessages = _context.UserMessages.Include(x => x.Messages).Where(x => x.SenderId == vm.CurrentUser.Id && x.ReceiverId == filteredSender.Id).ToList();
                vm.AdminFilteredMessages.AddRange(AdminFilteredSendedMessages);
            }

            return View(vm);
        }

        public async Task<IActionResult> AllChat()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            var appUser = await _userManager.GetUserAsync(User);
            string userId = appUser.Id;
            var user = await _userManager.FindByIdAsync(userId);
            if (ModelState.IsValid)
            {
                var check = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.ConfirmNewPassword);
                if (check.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                foreach (var err in check.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View();
        }
    }
}
