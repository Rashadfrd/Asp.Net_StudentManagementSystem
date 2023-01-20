using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetStudentManagementSystem.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _accessor; 
        public LayoutService(AppDbContext context, UserManager<AppUser> userManager, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _userManager = userManager;
            _context = context;
        }

        public async Task<UsersInfo> GetCurrentUserInfo()
        {
            AppUser currentUser = await _userManager.GetUserAsync(_accessor.HttpContext.User);
            UsersInfo userInfos = _context.UserInfos.Include(x=>x.Group).ThenInclude(x=>x.Department).FirstOrDefault(x => x.Email == currentUser.Email);
            return userInfos;
        }
        public async Task<Teacher> GetCurrentTeacherInfo()
        {
            AppUser currentUser = await _userManager.GetUserAsync(_accessor.HttpContext.User);
            Teacher teacher = _context.Teachers.Include(x=>x.Subject).FirstOrDefault(x => x.Email == currentUser.Email);
            return teacher;
        }

    }
}
