using Microsoft.EntityFrameworkCore;
using Student_GPA.Models;

namespace Student_GPA.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course?> GetCourseByCodeAsync(string courseCode)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseCode);
        }

        public async Task AddCourseAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await Task.CompletedTask;
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

    }
}
