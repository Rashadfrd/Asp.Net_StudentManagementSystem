namespace Asp.NetStudentManagementSystem.Models
{
    public class Department
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Group> Groups { get; set; }

    }
}
