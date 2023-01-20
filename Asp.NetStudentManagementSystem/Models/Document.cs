namespace Asp.NetStudentManagementSystem.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Duration { get; set; }
        public ICollection<UserDocument> UserDocuments { get; set; }

    }
}
