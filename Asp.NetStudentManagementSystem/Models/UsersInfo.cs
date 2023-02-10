using System.ComponentModel.DataAnnotations;

namespace Asp.NetStudentManagementSystem.Models
{
    public class UsersInfo
    {
        public int Id { get; set; }

        [Required, StringLength(30)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Surname { get; set; }

        public string Country { get; set; }
        public string Gender { get; set; }
        public int? EntranceYear { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(50)]
        public string FatherName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool isActive { get; set; }

        public Group? Group { get; set; }
        public int? GroupId { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }

    }
}
