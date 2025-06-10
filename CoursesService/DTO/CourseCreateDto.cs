using System.ComponentModel.DataAnnotations;

namespace CoursesService.DTO
{
    public class CourseCreateDto
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "A descrição é obrigatório")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "O professor é obrigatório")]
        public int? IdTeacher { get; set; }

    }

}