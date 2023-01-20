namespace Asp.NetStudentManagementSystem.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public LessonHour? LessonHour { get; set; }
        public int LessonHourId { get; set; }
        public LessonDay? LessonDay { get; set; }
        public int LessonDayId { get; set; }
        public int Room { get; set; }
        public Group? Group { get; set; }
        public int GroupId { get; set; }
        public Subject? Subject { get; set; }
        public int SubjectId { get; set; }
        public bool isDeleted { get; set; }

    }
}
