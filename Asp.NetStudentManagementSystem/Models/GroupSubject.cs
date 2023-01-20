namespace Asp.NetStudentManagementSystem.Models
{
    public class GroupSubject
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public int GroupId { get; set; }
        public Subject Subject { get; set; }
        public int SubjectId { get; set; }
    }
}
