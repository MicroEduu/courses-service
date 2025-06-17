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
            var res = await _service.GetAllAsync();
            if (res == null || !res.Any())
            {
                return NotFound("Nenhum curso cadastrado");
            }
            return Ok(res);
        }

        // Get by id: Admin, Professor, Aluno
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _service.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound("Curso não encontrado");
            }
            return Ok(course);
        }

        // Create: Admin, Professor
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Token inválido: Id do usuário não encontrado");

                var teacherId = int.Parse(userIdClaim.Value);

                var createdCourse = await _service.CreateAsync(dto, teacherId);

                return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, createdCourse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar curso: {ex.Message}");
            }
        }

        // Edit: Admin, Professor
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateCoursePartial(int id, [FromBody] CourseUpdateDto dto)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Token inválido: Id do usuário não encontrado");

                var userId = int.Parse(userIdClaim.Value);

                var course = await _service.GetByIdAsync(id);
                if (course == null)
                    return NotFound("Curso não encontrado");

                // Se não for admin, verifica se é o professor dono do curso
                if (!User.IsInRole("Admin") && course.IdTeacher != userId)
                    return Forbid("Apenas o professor responsável pode editar este curso.");

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

        // Delete: Admin, Professor
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Token inválido: Id do usuário não encontrado");

                var userId = int.Parse(userIdClaim.Value);

                var course = await _service.GetByIdAsync(id);
                if (course == null)
                    return NotFound("Curso não encontrado");

                // Se não for admin, verifica se é o professor dono do curso
                if (!User.IsInRole("Admin") && course.IdTeacher != userId)
                    return Forbid("Apenas o professor responsável pode excluir este curso.");

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

        [HttpPatch("increment-subscriber/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize] 
        public async Task<IActionResult> IncrementSubscriber(int id)
        {
            try
            {
                var course = await _service.GetByIdAsync(id);
                if (course == null)
                    return NotFound("Curso não encontrado");

                await _service.IncrementSubscriberAsync(id);

                return Ok("Número de inscritos atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar número de inscritos: {ex.Message}");
            }
        }


    }
}