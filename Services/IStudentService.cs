using Student_GPA.ViewModels;

namespace Student_GPA.Services
{
    public interface IStudentService
    {
        Task<List<StudentFormViewModel>> GetAllStudentsAsync();
        Task<StudentFormViewModel> GetStudentByIdAsync(int id);
        Task CreateStudentAsync(StudentFormViewModel model);
        Task UpdateStudentAsync(StudentFormViewModel model);
        Task DeleteStudentAsync(int id);
    }
}
