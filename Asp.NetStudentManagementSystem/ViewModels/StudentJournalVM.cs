using Asp.NetStudentManagementSystem.Models;

namespace Asp.NetStudentManagementSystem.ViewModels
{
    public class StudentJournalVM
    {
        public IEnumerable<GroupSubject> GroupSubjects;
        public IEnumerable<Attendance> Attendances;
    }
}
