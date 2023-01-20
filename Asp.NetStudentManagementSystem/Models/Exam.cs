namespace Asp.NetStudentManagementSystem.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Group? Group { get; set; }
        public int GroupId { get; set; }
        public Subject? Subject { get; set; }
        public int SubjectId { get; set; }
        public bool isDeleted { get; set; }
    }
}