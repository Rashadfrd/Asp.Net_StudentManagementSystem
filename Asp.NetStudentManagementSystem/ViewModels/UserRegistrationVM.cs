using System.ComponentModel.DataAnnotations;

namespace Asp.NetStudentManagementSystem.ViewModels
{
    public class UserRegistrationVM
    {
        [Required, StringLength(30)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Surname { get; set; }

        public string? Country { get; set; }
        public string? Gender { get; set; }
        public int? EntranceYear { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(50)]
        public string? FatherName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, StringLength(40), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, StringLength(40), DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public int GroupId { get; set; }
        public int SubjectId { get; set; }
        public bool isRemember { get; set; } = false;
    }
}
