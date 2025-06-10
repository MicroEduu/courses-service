namespace CoursesService.DTOs
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int IdTeacher { get; set; }
        public int NumberSubscribers { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
