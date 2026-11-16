using Microsoft.AspNetCore.Mvc;
using Student_GPA.Services;
using Student_GPA.ViewModels;

namespace Student_GPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound(new { message = "Course not found." });

            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseFormViewModel model)
        {
            ModelState.Remove(nameof(model.CourseCode));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.CreditHours < 1 || model.CreditHours > 3)
            {
                return BadRequest(new { message = "Credit hours must be between 1 and 3." });
            }

            await _courseService.CreateCourseAsync(model);
            return Ok(new { message = "Course added successfully!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] CourseFormViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            ModelState.Remove(nameof(model.CourseCode));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.CreditHours < 1 || model.CreditHours > 3)
            {
                return BadRequest(new { message = "Credit hours must be between 1 and 3." });
            }

            var existingCourse = await _courseService.GetCourseByIdAsync(id);
            if (existingCourse == null)
                return NotFound(new { message = "Course not found." });

            await _courseService.UpdateCourseAsync(model);
            return Ok(new { message = "Course updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCourse = await _courseService.GetCourseByIdAsync(id);
            if (existingCourse == null)
                return NotFound(new { message = "Course not found." });

            await _courseService.DeleteCourseAsync(id);
            return Ok(new { message = "Course deleted successfully!" });
        }
    }
}
