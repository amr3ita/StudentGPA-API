using System.ComponentModel.DataAnnotations;

namespace Student_GPA.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string CourseCode { get; set; }
        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(100)]
        public string CourseName { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "Credit hours must be between 1 and 3.")]
        public int CreditHours { get; set; }

        public List<StudentCourse> Enrollments { get; set; } = new List<StudentCourse> { };
    }
}
