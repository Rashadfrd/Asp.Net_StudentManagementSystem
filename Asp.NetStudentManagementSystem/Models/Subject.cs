namespace Asp.NetStudentManagementSystem.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Hours { get; set; }
        public ICollection<GroupSubject>? GroupSubjects { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
        public ICollection<Teacher>? Teachers { get; set; }

    }
}
