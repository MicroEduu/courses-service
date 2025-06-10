using System.ComponentModel.DataAnnotations;

namespace CoursesService.DTO
{
    public class CourseCreateDto
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string Description { get; set; } = null!;
    }
}
