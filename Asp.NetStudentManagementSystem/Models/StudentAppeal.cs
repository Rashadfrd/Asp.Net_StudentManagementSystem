namespace Asp.NetStudentManagementSystem.Models
{
    public class StudentAppeal
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Content { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowed { get; set; }

        public AppUser? AppUser { get; set; }
        public string? AppUserId { get; set; }
        public Subject? Subject { get; set; }
        public int SubjectId { get; set; }
    }
}
