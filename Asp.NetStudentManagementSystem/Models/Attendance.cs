using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.NetStudentManagementSystem.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public UsersInfo Student { get; set; }
        public int StudentId { get; set; }
        public Subject Subject { get; set; }
        public int SubjectId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public byte? Grade { get; set; }

        [NotMapped]
        public int GradeOrAbsent { get; set; }
    }
}
