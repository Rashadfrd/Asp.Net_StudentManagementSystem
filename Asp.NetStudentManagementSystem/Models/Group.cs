namespace Asp.NetStudentManagementSystem.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Department? Department { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<UsersInfo>? Students { get; set; }
        public ICollection<GroupSubject>? GroupSubjects { get; set; }
        public ICollection<Exam>? Exams { get; set; }
    }
}
