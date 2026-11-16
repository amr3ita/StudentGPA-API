using Student_GPA.Models;

namespace Student_GPA.Repositories
{
    public interface IStudentCourseRepository
    {
        Task<List<StudentCourse>> GetStudentEnrollmentsAsync(int studentId);
        Task<StudentCourse?> GetEnrollmentByIdAsync(int id);
        Task<StudentCourse?> GetEnrollmentByStudentAndCourseAsync(int studentId, int courseId);
        Task AddEnrollmentAsync(StudentCourse studentCourse);
        Task UpdateEnrollmentAsync(StudentCourse studentCourse);
        Task DeleteEnrollmentAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
