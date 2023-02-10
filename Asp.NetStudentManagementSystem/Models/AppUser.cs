using Microsoft.AspNetCore.Identity;

namespace Asp.NetStudentManagementSystem.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<UserDocument> UserDocuments { get; set; }
        public ICollection<StudentAppeal> StudentAppeals { get; set; }
    }
}
