using System.ComponentModel.DataAnnotations;

namespace Asp.NetStudentManagementSystem.ViewModels
{
    public class UserLoginVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, StringLength(40), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
