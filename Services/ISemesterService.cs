using Student_GPA.ViewModels;

namespace Student_GPA.Services
{
    public interface ISemesterService
    {
        Task<List<SemesterFormViewModel>> GetAllSemestersAsync();
        Task<SemesterFormViewModel?> GetSemesterByIdAsync(int id);
        Task CreateSemesterAsync(SemesterFormViewModel model);
        Task UpdateSemesterAsync(SemesterFormViewModel model);
        Task ToggleSemesterActivationAsync(int id);
        Task DeleteSemesterAsync(int id);
    }
}
