using Student_GPA.Models;
using Student_GPA.Repositories;
using Student_GPA.ViewModels;

namespace Student_GPA.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<CourseFormViewModel>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            return courses.Select(c => new CourseFormViewModel
            {
                Id = c.Id,
                CourseName = c.CourseName,
                CourseCode = c.CourseCode,
                CreditHours = c.CreditHours,
            }).ToList();
        }

        public async Task<CourseFormViewModel> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null) return null;
            return new CourseFormViewModel
            {
                Id = course.Id,
                CourseName = course.CourseName,
                CourseCode = course.CourseCode,
                CreditHours = course.CreditHours,
            };
        }

        public async Task CreateCourseAsync(CourseFormViewModel model)
        {
            string uniqueCode = await GenerateUniqueCourseCodeAsync();
            var course = new Course
            {
                CourseName = model.CourseName,
                CourseCode = uniqueCode,
                CreditHours = model.CreditHours
            };
            await _courseRepository.AddCourseAsync(course);
            await _courseRepository.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(CourseFormViewModel model)
        {
            var course = await _courseRepository.GetCourseByIdAsync(model.Id);
            if (course != null)
            {
                course.CourseName = model.CourseName;
                course.CreditHours = model.CreditHours;

                await _courseRepository.UpdateCourseAsync(course);
                await _courseRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _courseRepository.DeleteCourseAsync(id);
            await _courseRepository.SaveChangesAsync();
        }


        #region private functions
        private async Task<string> GenerateUniqueCourseCodeAsync()
        {
            string generatedCode;
            bool codeExists;

            do
            {
                var randomNum = new Random().Next(1000, 9999);
                generatedCode = $"CRS-{randomNum}";

                var existing = await _courseRepository.GetCourseByCodeAsync(generatedCode);
                codeExists = (existing != null);

            } while (codeExists);

            return generatedCode;
        }
        #endregion
    }
}
