using Microsoft.EntityFrameworkCore;
using Student_GPA.Models;

namespace Student_GPA.Repositories
{
    public class StudentCourseRepository : IStudentCourseRepository
    {
        private readonly AppDbContext _context;

        public StudentCourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<StudentCourse>> GetStudentEnrollmentsAsync(int studentId)
        {
            return _context.StudentCourses
                .Include(sc => sc.Course)
                .Include(sc => sc.Semester)
                .Where(sc => sc.StudentId == studentId)
                .ToListAsync();
        }

        public Task<StudentCourse?> GetEnrollmentByIdAsync(int id)
        {
            return _context.StudentCourses
                .Include(sc => sc.Course)
                .Include(sc => sc.Semester)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<StudentCourse?> GetEnrollmentByStudentAndCourseAsync(int studentId, int courseId)
        {
            return await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public async Task AddEnrollmentAsync(StudentCourse studentCourse)
        {
            await _context.StudentCourses.AddAsync(studentCourse);
        }

        public async Task UpdateEnrollmentAsync(StudentCourse studentCourse)
        {
            _context.StudentCourses.Update(studentCourse);
            await Task.CompletedTask;
        }

        public async Task DeleteEnrollmentAsync(int id)
        {
            var studentCourse = await _context.StudentCourses.FindAsync(id);
            if (studentCourse != null)
            {
                _context.StudentCourses.Remove(studentCourse);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
