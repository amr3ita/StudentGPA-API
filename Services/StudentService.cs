using Student_GPA.Models;
using Student_GPA.Repositories;
using Student_GPA.ViewModels;

namespace Student_GPA.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<StudentFormViewModel>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            return students.Select(s => new StudentFormViewModel
            {
                Id = s.Id,
                StudentCode = s.StudentCode,
                FullName = s.FullName,
            }).ToList();
        }

        public async Task<StudentFormViewModel> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null) return null;

            return new StudentFormViewModel
            {
                Id = student.Id,
                FullName = student.FullName,
                StudentCode = student.StudentCode
            };
        }

        public async Task CreateStudentAsync(StudentFormViewModel model)
        {
            string uniqueCode = await GenerateUniqueStudentCodeAsync();
            var student = new Student
            {
                StudentCode = uniqueCode,
                FullName = model.FullName
            };

            await _studentRepository.AddStudentAsync(student);
            await _studentRepository.SaveChangesAsync();
        }

        public async Task UpdateStudentAsync(StudentFormViewModel model)
        {
            var student = await _studentRepository.GetStudentByIdAsync(model.Id);
            if (student != null)
            {
                student.FullName = model.FullName;
            }

            await _studentRepository.UpdateStudentAsync(student);
            await _studentRepository.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteStudentAsync(id);
            await _studentRepository.SaveChangesAsync();
        }


        #region private functions
        private async Task<string> GenerateUniqueStudentCodeAsync()
        {
            string generatedCode;
            bool codeExists;

            do
            {
                var randomNum = new Random().Next(1000, 9999);
                generatedCode = $"STU-{DateTime.Now.Year}-{randomNum}";

                var existing = await _studentRepository.GetStudentByCodeAsync(generatedCode);
                codeExists = (existing != null);

            } while (codeExists);

            return generatedCode;
        }
        #endregion
    }
}
