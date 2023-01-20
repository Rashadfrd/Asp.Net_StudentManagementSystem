using Asp.NetStudentManagementSystem.Models;

namespace Asp.NetStudentManagementSystem.ViewModels
{
    public class RedocVM
    {
        public UsersInfo Student { get; set; }
        public IEnumerable<Document> Documents { get; set; }
    }
}
