using Microsoft.EntityFrameworkCore;
using Student_GPA.Models;

namespace Student_GPA.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly AppDbContext _context;

        public SemesterRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Semester>> GetAllSemestersAsync()
        {
            return await _context.Semesters.ToListAsync();
        }

        public async Task<Semester?> GetSemesterByIdAsync(int id)
        {
            return await _context.Semesters.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Semester?> GetActiveSemesterAsync()
        {
            return await _context.Semesters.FirstOrDefaultAsync(s => s.IsActive == true);
        }
        public async Task AddSemesterAsync(Semester semester)
        {
            await _context.Semesters.AddAsync(semester);
        }

        public async Task UpdateSemesterAsync(Semester semester)
        {
            _context.Semesters.Update(semester);
            await Task.CompletedTask;
        }
        public async Task DeleteSemesterAsync(int id)
        {
            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Id == id);
            if (semester != null)
            {
                _context.Semesters.Remove(semester);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

    }
}
