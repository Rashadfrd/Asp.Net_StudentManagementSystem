namespace Asp.NetStudentManagementSystem.Models
{
    public class UserDocument
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public Document Document { get; set; }
        public int DocumentId { get; set; }
        public string ToWhere { get; set; }
        public bool isReady { get; set; }
    }
}
