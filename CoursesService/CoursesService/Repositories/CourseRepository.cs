using CoursesService.Data;
using CoursesService.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursesService.Repositories
{
    public class CourseRepository
    {
        private readonly DataContext _context;

        public CourseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync() => await _context.Courses.ToListAsync();
        public async Task<Course?> GetByIdAsync(int id) => await _context.Courses.FindAsync(id);
        public async Task CreateAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
