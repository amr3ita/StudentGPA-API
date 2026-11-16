using Microsoft.EntityFrameworkCore;
using Student_GPA.Models;

namespace Student_GPA.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.Include(s => s.Enrollments).ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student?> GetStudentByCodeAsync(string studentCode)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.StudentCode == studentCode);
        }

        public async Task AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }
        public async Task UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await Task.CompletedTask;
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
