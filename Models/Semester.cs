using System.ComponentModel.DataAnnotations;

namespace Student_GPA.Models
{
    public class Semester
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Semester name is required.")]
        [StringLength(100)]
        public string SemesterName { get; set; }

        public bool IsActive { get; set; }

        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
