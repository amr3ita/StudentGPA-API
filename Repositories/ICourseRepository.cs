using Student_GPA.Models;

namespace Student_GPA.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<Course?> GetCourseByCodeAsync(string courseCode);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
