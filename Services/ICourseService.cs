using Student_GPA.ViewModels;

namespace Student_GPA.Services
{
    public interface ICourseService
    {
        Task<List<CourseFormViewModel>> GetAllCoursesAsync();
        Task<CourseFormViewModel> GetCourseByIdAsync(int id);
        Task CreateCourseAsync(CourseFormViewModel model);
        Task UpdateCourseAsync(CourseFormViewModel model);
        Task DeleteCourseAsync(int id);
    }
}
