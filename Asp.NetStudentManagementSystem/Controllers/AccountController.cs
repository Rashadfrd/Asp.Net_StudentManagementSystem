using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Enums;
using Asp.NetStudentManagementSystem.Models;
using Asp.NetStudentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Asp.NetStudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signInManager { get; }
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        public const string templatePath = @"EmailTemplate/{0}.html";
        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 IConfiguration configuration,
                                 RoleManager<IdentityRole> roleManager,
                                 AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM user)
        {
            if (!ModelState.IsValid) return View();
            var existedUser = await _userManager.FindByEmailAsync(user.Email);
            if(existedUser is null)
            {
                ModelState.AddModelError("Email", "Email və ya parol yanlışdır");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(existedUser, user.Password, false, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "You made too many attemtps. Wait until -> " + existedUser.LockoutEnd?.AddHours(4).DateTime.ToString("MM/dd/yyyy HH:mm:ss"));
                    return View();
                }
                ModelState.AddModelError("", "Email or password is wrong");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationVM user)
        {
            if (!ModelState.IsValid) return View();
            var existUser = await _userManager.FindByEmailAsync(user.Email);
            if (existUser != null)
            {
                ModelState.AddModelError("Email", "Bu email vasitəsilə hesab artıq mövcuddur.");
                return View();
            }
            UsersInfo userInfo = new()
            {
                Name = user.Name,
                Surname = user.Surname,
                Gender = user.Gender,
                isActive = true,
                Email = user.Email,
                Country = user.Country,
                FatherName = user.FatherName,
                DateOfBirth = user.DateOfBirth,
            };
            await _context.UserInfos.AddAsync(userInfo);
            await _context.SaveChangesAsync();
            AppUser userCreate = new()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.Email.Substring(0,user.Email.IndexOf("@"))
            };
            var result = await _userManager.CreateAsync(userCreate, user.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.ToString());
                }
                return View();
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userCreate);
            if (!string.IsNullOrEmpty(token))
            {
                SendEmailConfirmationEmail(userCreate, "Sistemimizdən qeydiyyatdan keçdiyiniz üçün təşəkkürlər :) !", token);
                ViewBag.IsEmailSent = true;
            }
            return View();
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            }
        }

        public async Task<IActionResult> CreateSA()
        {
            AppUser superAdmin = new AppUser { Name = "Rashad", Surname = "Ferhadzade", UserName = "SuperAdmin", Email = "ferhadzaderesad@gmail.com", EmailConfirmed = true, IsAdmin = true };
            var result = await _userManager.CreateAsync(superAdmin, "Admin123");
            if (!result.Succeeded)
            {
                var err = "";
                foreach (var item in result.Errors)
                {
                    err += item;
                }
                return Content(err);
            }
            await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync(superAdmin.Email), Roles.SuperAdmin.ToString());
            return Ok(superAdmin);
        }


        public async Task<IActionResult> CreateTeacher()
        {
            AppUser teacher = new AppUser { Name = "Samir", Surname = "Canavar", UserName = "samirh01", Email = "samirh01@gmail.com", EmailConfirmed = true, IsAdmin = true };

            var result = await _userManager.CreateAsync(teacher, "Admin123");
            if (!result.Succeeded)
            {
                var err = "";
                foreach (var item in result.Errors)
                {
                    err += item;
                }
                return Content(err);
            }
            await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync(teacher.Email), Roles.Admin.ToString());
            _context.Teachers.Add(new() { Name = "Samir", Surname = "Canavar", Email = "samirh01@gmail.com", DateOfBirth = new DateTime(1999, 01, 01), SubjectId = 4, isActive = true });
            _context.SaveChanges();
            return Ok(teacher);
        }


        private async void SendEmailConfirmationEmail(AppUser user, string subject, string token)
        {
            string myEmail = "ferhadzaderesad@gmail.com";
            string password = "wgzykxvjedjemise";

            var from = new MailAddress(myEmail, "Student Support");
            var to = new MailAddress(user.Email);

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(myEmail, password)
            };
            using (var message = new MailMessage(from, to) { Subject = subject, Body = await UpdateNamePlaceHolder(GetEmailBody("EmailConfirmation"), user, token), IsBodyHtml = true })
            {
                smtp.Send(message);
            }
        }



        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var check = await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
                if (check.Succeeded)
                {
                    ViewBag.IsVerified = true;
                }
            }
            return View();
        }
        private string GetEmailBody(string template)
        {
            var body = System.IO.File.ReadAllText(string.Format(templatePath, template));
            return body;
        }

        private async Task<string> UpdateNamePlaceHolder(string text, AppUser user, string token)
        {
            //string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            //string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

            string appDomain = "https://localhost:7189/";
            string confirmationLink = "confirm-email?uid={0}&token={1}";

            string placeHolderLink = "{{Link}}";
            string placeHolder = "{{Username}}";
            if (text.Contains(placeHolder))
            {
                text = text.Replace(placeHolder, user.Name);
            }
            if (text.Contains(placeHolderLink))
            {
                text = text.Replace(placeHolderLink, string.Format(appDomain + confirmationLink, user.Id, token));
            }
            return text;
        }
    }
}
