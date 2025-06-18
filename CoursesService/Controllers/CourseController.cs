using CoursesService.DTO;
using CoursesService.DTOs;
using CoursesService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        // Get all: Admin, Professor, Aluno
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _service.GetAllAsync();

            if (courses == null || !courses.Any())
                return NoContent();

            return Ok(new { message = "Cursos encontrados com sucesso.", data = courses });
        }

        // Get by id: Admin, Professor, Aluno
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _service.GetByIdAsync(id);

            return course == null
                ? NotFound(new { message = "Curso não encontrado." })
                : Ok(new { message = "Curso encontrado com sucesso.", data = course });
        }

        // Create: Admin, Professor
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
        {
            if (User.IsInRole("Student"))
                return StatusCode(403, new { message = "Alunos podem somente visualizar os cursos." });

            if (!ModelState.IsValid)
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "Token inválido: Id do usuário não encontrado." });

            var teacherId = int.Parse(userIdClaim.Value);

            try
            {
                var created = await _service.CreateAsync(dto, teacherId);
                return CreatedAtAction(nameof(GetCourseById), new { id = created.Id },
                    new { message = "Curso criado com sucesso.", data = created });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao criar curso.", details = ex.Message });
            }
        }

        // Edit: Admin, Professor
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateCoursePartial(int id, [FromBody] CourseUpdateDto dto)
        {
            if (User.IsInRole("Student"))
                return StatusCode(403, new { message = "Alunos podem somente visualizar os cursos." });

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "Token inválido: Id do usuário não encontrado." });

            var userId = int.Parse(userIdClaim.Value);
            var course = await _service.GetByIdAsync(id);

            if (course == null)
                return NotFound(new { message = "Curso não encontrado." });

            if (!User.IsInRole("Admin") && course.IdTeacher != userId)
                return StatusCode(403, new { message = "Apenas o professor responsável pode editar este curso." });

            try
            {
                var updated = await _service.UpdatePartialAsync(id, dto);
                return Ok(new { message = "Curso atualizado com sucesso.", data = updated });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = "Erro de validação.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao atualizar curso.", details = ex.Message });
            }
        }

        // Delete: Admin, Professor
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (User.IsInRole("Student"))
                return StatusCode(403, new { message = "Alunos podem somente visualizar os cursos." });

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "Token inválido: Id do usuário não encontrado." });

            var userId = int.Parse(userIdClaim.Value);
            var course = await _service.GetByIdAsync(id);

            if (course == null)
                return NotFound(new { message = "Curso não encontrado." });

            if (!User.IsInRole("Admin") && course.IdTeacher != userId)
                return StatusCode(403, new { message = "Apenas o professor responsável pode excluir este curso." });

            try
            {
                var result = await _service.DeleteAsync(id);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao excluir curso.", details = ex.Message });
            }
        }

        // Increment Subscriber
        [HttpPatch("increment-subscriber/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        public async Task<IActionResult> IncrementSubscriber(int id)
        {
            var course = await _service.GetByIdAsync(id);
            if (course == null)
                return NotFound(new { message = "Curso não encontrado." });

            try
            {
                await _service.IncrementSubscriberAsync(id);
                return Ok(new { message = "Número de inscritos incrementado com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao atualizar número de inscritos.", details = ex.Message });
            }
        }
    }
}
