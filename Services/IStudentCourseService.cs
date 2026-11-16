using Student_GPA.Enum;
using Student_GPA.ViewModels;

namespace Student_GPA.Services
{
    public interface IStudentCourseService
    {
        Task<StudentTranscriptViewModel> GetStudentTranscriptAsync(int studentId);
        Task<List<CourseSelectionViewModel>> GetAvailableCoursesForRegistrationAsync(int studentId);
        Task<(bool Success, string Message)> RegisterCoursesAsync(int studentId, List<int> selectedCourseIds);
        Task UpdateGradeOrStatusAsync(int enrollmentId, decimal? score, SpecialStatus status);
        Task DeleteEnrollmentAsync(int enrollmentId);
    }
}
