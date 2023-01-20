using System.ComponentModel.DataAnnotations;

namespace Asp.NetStudentManagementSystem.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public Subject? Subject { get; set; }
        public int SubjectId { get;set; }

        public bool isActive { get; set; }
    }
}
