namespace CoursesService.DTO
{
    public class CourseCreateDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int IdTeacher { get; set; }

    }

}