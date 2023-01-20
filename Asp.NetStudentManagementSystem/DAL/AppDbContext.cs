using Asp.NetStudentManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetStudentManagementSystem.DAL
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UsersInfo> UserInfos { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<GroupSubject> GroupSubjects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<UserDocument> UserDocuments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<LessonHour> LessonHours { get; set; }
        public DbSet<LessonDay> LessonDays { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserMessages> UserMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }

    }
}
