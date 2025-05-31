using CoursesService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CoursesService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
    }
}
