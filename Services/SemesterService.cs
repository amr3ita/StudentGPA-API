using Student_GPA.Models;
using Student_GPA.Repositories;
using Student_GPA.ViewModels;

namespace Student_GPA.Services
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;

        public SemesterService(ISemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;
        }

        public async Task<List<SemesterFormViewModel>> GetAllSemestersAsync()
        {
            var semesters = await _semesterRepository.GetAllSemestersAsync();
            return semesters.Select(s => new SemesterFormViewModel
            {
                Id = s.Id,
                SemesterName = s.SemesterName,
                IsActive = s.IsActive,
            }).ToList();
        }

        public async Task<SemesterFormViewModel?> GetSemesterByIdAsync(int id)
        {
            var semester = await _semesterRepository.GetSemesterByIdAsync(id);
            if (semester == null) return null;
            return new SemesterFormViewModel
            {
                Id = semester.Id,
                SemesterName = semester.SemesterName,
                IsActive = semester.IsActive,
            };
        }

        public async Task CreateSemesterAsync(SemesterFormViewModel model)
        {
            // make all other semesters not active
            if (model.IsActive)
            {
                var allSemesters = await _semesterRepository.GetAllSemestersAsync();
                foreach (var sem in allSemesters)
                {
                    sem.IsActive = false;
                    await _semesterRepository.UpdateSemesterAsync(sem);
                }
            }

            // add semester
            var semester = new Semester
            {
                SemesterName = model.SemesterName,
                IsActive = model.IsActive
            };

            await _semesterRepository.AddSemesterAsync(semester);
            await _semesterRepository.SaveChangesAsync();
        }
        public async Task UpdateSemesterAsync(SemesterFormViewModel model)
        {
            var semester = await _semesterRepository.GetSemesterByIdAsync(model.Id);
            if (semester != null)
            {
                // to activate a semester => first make all other semesters non active
                if (model.IsActive && !semester.IsActive)
                {
                    var allSemesters = await _semesterRepository.GetAllSemestersAsync();
                    foreach (var sem in allSemesters)
                    {
                        sem.IsActive = false;
                        await _semesterRepository.UpdateSemesterAsync(sem);
                    }
                }

                semester.SemesterName = model.SemesterName;
                semester.IsActive = model.IsActive;

                await _semesterRepository.UpdateSemesterAsync(semester);
                await _semesterRepository.SaveChangesAsync();
            }


        }
        public async Task ToggleSemesterActivationAsync(int id)
        {
            var semester = await _semesterRepository.GetSemesterByIdAsync(id);
            if (semester == null) return;

            // to activate a semester => first make all other semesters non active
            if (!semester.IsActive)
            {
                var allSemesters = await _semesterRepository.GetAllSemestersAsync();
                foreach (var sem in allSemesters)
                {
                    sem.IsActive = false;
                    await _semesterRepository.UpdateSemesterAsync(sem);
                }

                semester.IsActive = true;
            }
            else
            {
                semester.IsActive = false;
            }

            await _semesterRepository.UpdateSemesterAsync(semester);
            await _semesterRepository.SaveChangesAsync();
        }

        public async Task DeleteSemesterAsync(int id)
        {
            await _semesterRepository.DeleteSemesterAsync(id);
            await _semesterRepository.SaveChangesAsync();
        }
    }
}
