using Asp.NetStudentManagementSystem.Models;

namespace Asp.NetStudentManagementSystem.ViewModels
{
    public class HomeVM
    {
        public Group CurrentGroup { get; set; }
        public Subject Subject { get; set; }
        public UsersInfo CurrentStudent { get; set; }
        public List<Attendance> Attendance { get; set; }
        public List<UsersInfo> Students { get; set; }
        public List<GroupSubject> GroupSubjects { get; set; }
        public DateTime Date { get; set; }
        public AppUser CurrentUser { get; set; }
        public List<UserMessages> UserMessages { get; set; }
        public List<AppUser> AllUsers { get; set; }
        public IEnumerable<UserMessages> AdminMessages { get; set; }
        public List<UserMessages> AdminFilteredMessages { get; set; }
    }
}
