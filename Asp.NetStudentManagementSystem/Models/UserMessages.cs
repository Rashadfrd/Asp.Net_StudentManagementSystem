namespace Asp.NetStudentManagementSystem.Models
{
    public class UserMessages
    {
        public int Id { get; set; }
        public AppUser Sender { get; set; }
        public string SenderId { get; set; }
        public AppUser? Receiver { get; set; }
        public string? ReceiverId { get; set; }
        public Message Messages { get; set; }
        public int MessageId { get; set; }
        public DateTime SentDate { get; set; }

    }
}
