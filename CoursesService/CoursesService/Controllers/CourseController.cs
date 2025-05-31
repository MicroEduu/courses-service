using CoursesService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoursesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var res = await _service.GetAllAsync();
            if (res == null || !res.Any())
            {
                return NotFound("Nenhum curso cadastrado");
            }
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _service.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound("Curso não encontrado");
            }
            return Ok(course);
        }

    }
}