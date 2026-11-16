using Microsoft.AspNetCore.Mvc;
using Student_GPA.Services;
using Student_GPA.ViewModels;

namespace Student_GPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound(new { message = "Student not found." });

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentFormViewModel model)
        {
            ModelState.Remove(nameof(model.StudentCode));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _studentService.CreateStudentAsync(model);
            return Ok(new { message = "Student added successfully!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] StudentFormViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            ModelState.Remove(nameof(model.StudentCode));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingStudent = await _studentService.GetStudentByIdAsync(id);
            if (existingStudent == null)
                return NotFound(new { message = "Student not found." });

            await _studentService.UpdateStudentAsync(model);
            return Ok(new { message = "Student updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingStudent = await _studentService.GetStudentByIdAsync(id);
            if (existingStudent == null)
                return NotFound(new { message = "Student not found." });

            await _studentService.DeleteStudentAsync(id);
            return Ok(new { message = "Student deleted successfully!" });
        }
    }
}
