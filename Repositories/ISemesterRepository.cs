using Student_GPA.Models;

namespace Student_GPA.Repositories
{
    public interface ISemesterRepository
    {
        Task<List<Semester>> GetAllSemestersAsync();
        Task<Semester?> GetSemesterByIdAsync(int id);
        Task<Semester?> GetActiveSemesterAsync();
        Task AddSemesterAsync(Semester semester);
        Task UpdateSemesterAsync(Semester semester);
        Task DeleteSemesterAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
