using Microsoft.AspNetCore.Mvc;
using Student_GPA.Services;
using Student_GPA.ViewModels;

namespace Student_GPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IStudentCourseService _studentCourseService;

        public EnrollmentsController(IStudentCourseService studentCourseService)
        {
            _studentCourseService = studentCourseService;
        }


        [HttpGet("transcript/{studentId}")]
        public async Task<IActionResult> GetTranscript(int studentId)
        {
            var transcript = await _studentCourseService.GetStudentTranscriptAsync(studentId);
            if (transcript == null)
                return NotFound(new { message = "Student not found." });

            return Ok(transcript);
        }

        [HttpGet("available-courses/{studentId}")]
        public async Task<IActionResult> GetAvailableCourses(int studentId)
        {
            var availableCourses = await _studentCourseService.GetAvailableCoursesForRegistrationAsync(studentId);
            return Ok(availableCourses);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCourses([FromBody] RegisterRequestViewModel model)
        {
            var result = await _studentCourseService.RegisterCoursesAsync(model.StudentId, model.SelectedCourseIds);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("update-grade")]
        public async Task<IActionResult> UpdateGrade([FromBody] GradeUpdateViewModel model)
        {
            await _studentCourseService.UpdateGradeOrStatusAsync(model.EnrollmentId, model.Score, model.Status);
            return Ok(new { message = "Grade / Status updated successfully!" });
        }

        [HttpDelete("drop-course/{enrollmentId}")]
        public async Task<IActionResult> DropCourse(int enrollmentId)
        {
            await _studentCourseService.DeleteEnrollmentAsync(enrollmentId);
            return Ok(new { message = "Course dropped successfully!" });
        }
    }
}
