using Microsoft.EntityFrameworkCore;
using Student_GPA.Enum;
using Student_GPA.Models;
using Student_GPA.Repositories;
using Student_GPA.ViewModels;

namespace Student_GPA.Services
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly AppDbContext _context;

        public StudentCourseService(
            IStudentCourseRepository studentCourseRepository,
            ICourseRepository courseRepository,
            ISemesterRepository semesterRepository,
            IStudentRepository studentRepository,
            AppDbContext context)
        {
            _studentCourseRepository = studentCourseRepository;
            _courseRepository = courseRepository;
            _semesterRepository = semesterRepository;
            _studentRepository = studentRepository;
            _context = context;
        }


        public async Task<StudentTranscriptViewModel> GetStudentTranscriptAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) return null;

            var enrollments = await _studentCourseRepository.GetStudentEnrollmentsAsync(studentId);
            var activeSemester = await _semesterRepository.GetActiveSemesterAsync();
            var gradingScales = await _context.GradingScales.ToListAsync();


            // get student enrollments with details (grades, points, ......)
            var enrollmentItems = enrollments.Select(e =>
            {
                string letter = "-";
                decimal points = 0.0m;

                if (e.SpecialStatus == SpecialStatus.None && e.NumericalScore.HasValue)
                {
                    var scale = gradingScales.FirstOrDefault(gs => e.NumericalScore.Value >= gs.FromGrade && e.NumericalScore.Value < gs.ToGrade);
                    if (scale != null)
                    {
                        letter = scale.Grade;
                        points = scale.Points;
                    }
                }
                else if (e.SpecialStatus == SpecialStatus.ABS)
                {
                    letter = "ABS";
                    points = 0.0m;
                }
                else if (e.SpecialStatus == SpecialStatus.I)
                {
                    letter = "I";
                }
                else if (e.SpecialStatus == SpecialStatus.W)
                {
                    letter = "W";
                }

                return new
                {
                    e.SemesterId,
                    e.Semester.SemesterName,
                    e.Semester.IsActive,
                    Item = new EnrollmentItemViewModel
                    {
                        Id = e.Id,
                        CourseCode = e.Course.CourseCode,
                        CourseName = e.Course.CourseName,
                        CreditHours = e.Course.CreditHours,
                        NumericalScore = e.NumericalScore,
                        LetterGrade = letter,
                        Points = points,
                        Status = e.SpecialStatus.ToString()
                    }
                };
            }).ToList();

            // group each semester
            var semesterGroups = enrollmentItems
                .GroupBy(x => new { x.SemesterId, x.SemesterName, x.IsActive })
                .Select(g => new SemesterTranscriptViewModel
                {
                    SemesterId = g.Key.SemesterId,
                    SemesterName = g.Key.SemesterName,
                    IsActive = g.Key.IsActive,
                    Enrollments = g.Select(e => e.Item).ToList()
                }).ToList();

            // calc GPA
            decimal totalQualityPoints = 0;
            int totalEligibleHours = 0;
            int totalEarnedHours = 0;


            foreach (var sem in semesterGroups)
            {
                int semHours = 0;
                decimal semQualityPoints = 0;

                foreach (var item in sem.Enrollments)
                {
                    if (item.Status == "I" || item.Status == "W") continue;

                    semHours += item.CreditHours;
                    if (item.Points > 0 || item.Status == "ABS")
                    {
                        semQualityPoints += item.Points * item.CreditHours;
                    }

                    if (item.Points > 0)
                    {
                        totalEarnedHours += item.CreditHours;
                    }
                }

                sem.TotalSemesterHours = semHours;
                sem.SemesterGPA = semHours > 0 ? Math.Round(semQualityPoints / semHours, 2) : 0.0m;

                totalQualityPoints += semQualityPoints;
                totalEligibleHours += semHours;
            }

            decimal cumulativeGPA = totalEligibleHours > 0 ? Math.Round(totalQualityPoints / totalEligibleHours, 2) : 0.0m;

            return new StudentTranscriptViewModel
            {
                StudentId = student.Id,
                StudentCode = student.StudentCode,
                FullName = student.FullName,
                CumulativeGPA = cumulativeGPA,
                TotalEarnedHours = totalEarnedHours,
                IsRegistrationOpen = (activeSemester != null),
                ActiveSemesterName = activeSemester?.SemesterName,
                Semesters = semesterGroups
            };
        }

        public async Task<List<CourseSelectionViewModel>> GetAvailableCoursesForRegistrationAsync(int studentId)
        {
            var allCourses = await _courseRepository.GetAllCoursesAsync();
            var studentEnrollments = await _studentCourseRepository.GetStudentEnrollmentsAsync(studentId);

            var coursesToExclude = new HashSet<int>();
            var activeSemester = await _semesterRepository.GetActiveSemesterAsync();

            foreach (var enrollment in studentEnrollments)
            {
                if (activeSemester != null && enrollment.SemesterId == activeSemester.Id)
                {
                    coursesToExclude.Add(enrollment.CourseId);
                }

                if (enrollment.SpecialStatus == SpecialStatus.None)
                {
                    if (enrollment.NumericalScore.HasValue && enrollment.NumericalScore.Value >= 50)
                    {
                        coursesToExclude.Add(enrollment.CourseId);
                    }
                }
                else if (enrollment.SpecialStatus == SpecialStatus.W || enrollment.SpecialStatus == SpecialStatus.I)
                {

                }
            }

            var availableCourses = allCourses
                .Where(c => !coursesToExclude.Contains(c.Id))
                .Select(c => new CourseSelectionViewModel
                {
                    Id = c.Id,
                    CourseCode = c.CourseCode,
                    CourseName = c.CourseName,
                    CreditHours = c.CreditHours
                })
                .ToList();

            return availableCourses;
        }


        public async Task<(bool Success, string Message)> RegisterCoursesAsync(int studentId, List<int> selectedCourseIds)
        {
            if (selectedCourseIds == null || !selectedCourseIds.Any())
            {
                return (false, "Please select at least one course to register.");
            }

            // remove any dublicates
            var distinctCourseIds = selectedCourseIds.Distinct().ToList();

            // check if any semester active for registeration
            var activeSemester = await _semesterRepository.GetActiveSemesterAsync();
            if (activeSemester == null)
            {
                return (false, "Registration is currently closed. No active semester found.");
            }

            var studentEnrollments = await _studentCourseRepository.GetStudentEnrollmentsAsync(studentId);
            var alreadyTakenIds = studentEnrollments.Select(se => se.CourseId).ToHashSet();
            if (distinctCourseIds.Any(id => alreadyTakenIds.Contains(id)))
            {
                return (false, "One or more selected courses have already been taken or registered by this student.");
            }

            var allCourses = await _courseRepository.GetAllCoursesAsync();
            var selectedCourses = allCourses.Where(c => distinctCourseIds.Contains(c.Id)).ToList();

            if (selectedCourseIds.Count != distinctCourseIds.Count)
            {
                return (false, "One or more selected courses are invalid.");
            }

            // calc credit hours for any old registeration for the same semester
            int existingSemesterHours = studentEnrollments
                .Where(e => e.SemesterId == activeSemester.Id)
                .Sum(e => e.Course.CreditHours);

            int newSelectedHours = selectedCourses.Sum(c => c.CreditHours);
            int totalSemesterHours = existingSemesterHours + newSelectedHours;

            if (totalSemesterHours < 12)
            {
                return (false, $"Registration failed! Minimum allowed credit hours per semester is 12 hours (Total selected: {totalSemesterHours} hours).");
            }
            else if (totalSemesterHours > 18)
            {
                return (false, $"Registration failed! Maximum allowed credit hours per semester is 18 hours (Total selected: {totalSemesterHours} hours).");
            }

            foreach (var course in selectedCourses)
            {
                var enrollment = new StudentCourse
                {
                    StudentId = studentId,
                    CourseId = course.Id,
                    SemesterId = activeSemester.Id,
                    SpecialStatus = SpecialStatus.None,
                    NumericalScore = null
                };
                await _studentCourseRepository.AddEnrollmentAsync(enrollment);
            }

            await _studentCourseRepository.SaveChangesAsync();
            return (true, $"Successfully registered {selectedCourses.Count} courses ({newSelectedHours} credit hours)!");
        }

        public async Task UpdateGradeOrStatusAsync(int enrollmentId, decimal? score, SpecialStatus status)
        {
            var enrollment = await _studentCourseRepository.GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment != null)
            {
                if (status != SpecialStatus.None)
                {
                    enrollment.NumericalScore = null;
                }
                else
                {
                    enrollment.NumericalScore = score;
                }
                enrollment.SpecialStatus = status;

                await _studentCourseRepository.UpdateEnrollmentAsync(enrollment);
                await _studentCourseRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteEnrollmentAsync(int enrollmentId)
        {
            await _studentCourseRepository.DeleteEnrollmentAsync(enrollmentId);
            await _studentCourseRepository.SaveChangesAsync();
        }
    }
}
