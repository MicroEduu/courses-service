using CoursesService.DTO;
using CoursesService.DTOs;
using CoursesService.Models;
using CoursesService.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CoursesService.Services
{
    public class CourseService
    {
        private readonly CourseRepository _repository;

        public CourseService(CourseRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CourseReadDto>> GetAllAsync()
        {
            try
            {
                var courses = await _repository.GetAllAsync();

                if (courses == null || !courses.Any())
                {
                    return new List<CourseReadDto>();
                }

                return courses.Select(c => new CourseReadDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    IdTeacher = c.IdTeacher,
                    NumberSubscribers = c.NumberSubscribers,
                    CreatedAt = c.CreatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao buscar os cursos", ex);
            }

        }

        public async Task<CourseReadDto?> GetByIdAsync(int id)
        {
            try
            {
                var course = await _repository.GetByIdAsync(id);

                if (course == null)
                {
                    return null;
                }

                return new CourseReadDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    IdTeacher = course.IdTeacher,
                    NumberSubscribers = course.NumberSubscribers,
                    CreatedAt = course.CreatedAt
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao busca o curso", ex);
            }
        }

        public async Task<CourseReadDto> CreateAsync(CourseCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("O título é obrigatório");
            }

            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                throw new ArgumentException("A descrição é obrigatória");
            }

            if (!dto.IdTeacher.HasValue)
            {
                throw new ArgumentException("O professor é obrigatório");
            }

            var course = new Course(dto.Title, dto.Description)
            {
                IdTeacher = dto.IdTeacher.Value,
                NumberSubscribers = 0,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _repository.CreateAsync(course);

                return new CourseReadDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    IdTeacher = course.IdTeacher,
                    NumberSubscribers = course.NumberSubscribers,
                    CreatedAt = course.CreatedAt
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar o curso", ex);
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            var course = await _repository.GetByIdAsync(id);

            if (course == null)
            {
                throw new KeyNotFoundException("Curso não encontrado");
            }

            try
            {
                await _repository.DeleteAsync(id);
                return "Curso deletado com sucesso";
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao deletar curso", ex);
            }
        }

    }
}