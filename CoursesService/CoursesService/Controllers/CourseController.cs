using CoursesService.DTO;
using CoursesService.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCourse = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, createdCourse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar curso: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var message = await _service.DeleteAsync(id);
                return Ok(message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Curso não encontrado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar curso: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCoursePartial(int id, [FromBody] CourseUpdateDto dto)
        {
            try
            {
                var updatedCourse = await _service.UpdatePartialAsync(id, dto);
                return Ok(updatedCourse);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Curso não encontrado");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar curso: {ex.Message}");
            }
        }
    }
}