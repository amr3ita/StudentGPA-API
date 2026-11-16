using System.ComponentModel.DataAnnotations;

namespace Student_GPA.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string StudentCode { get; set; }
        [Required(ErrorMessage = "Student name is required.")]
        [StringLength(150)]
        public string FullName { get; set; }

        public List<StudentCourse> Enrollments { get; set; } = new List<StudentCourse> { };
    }
}
