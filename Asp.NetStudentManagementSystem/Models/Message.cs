using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.NetStudentManagementSystem.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Content { get; set; }
        public ICollection<UserMessages> UserMessages { get; set; }
    }
}
