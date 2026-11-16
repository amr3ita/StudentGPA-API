using Microsoft.AspNetCore.Mvc;
using Student_GPA.Services;
using Student_GPA.ViewModels;

namespace Student_GPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemestersController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var semesters = await _semesterService.GetAllSemestersAsync();
            return Ok(semesters);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var semester = await _semesterService.GetSemesterByIdAsync(id);
            if (semester == null)
                return NotFound(new { message = "Semester not found." });

            return Ok(semester);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SemesterFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _semesterService.CreateSemesterAsync(model);
            return Ok(new { message = "Semester added successfully!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] SemesterFormViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSemester = await _semesterService.GetSemesterByIdAsync(id);
            if (existingSemester == null)
                return NotFound(new { message = "Semester not found." });

            await _semesterService.UpdateSemesterAsync(model);
            return Ok(new { message = "Semester updated successfully!" });
        }

        [HttpPost("toggle-activation/{id}")]
        public async Task<IActionResult> ToggleActivation(int id)
        {
            var existingSemester = await _semesterService.GetSemesterByIdAsync(id);
            if (existingSemester == null)
                return NotFound(new { message = "Semester not found." });

            await _semesterService.ToggleSemesterActivationAsync(id);
            return Ok(new { message = "Semester status updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingSemester = await _semesterService.GetSemesterByIdAsync(id);
            if (existingSemester == null)
                return NotFound(new { message = "Semester not found." });

            await _semesterService.DeleteSemesterAsync(id);
            return Ok(new { message = "Semester deleted successfully!" });
        }


    }
}
