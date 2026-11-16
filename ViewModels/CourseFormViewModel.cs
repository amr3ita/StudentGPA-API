using System.ComponentModel.DataAnnotations;

namespace Student_GPA.ViewModels
{
    public class CourseFormViewModel
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(150)]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Credit hours are required.")]
        [Range(1, 3, ErrorMessage = "Credit hours must be between 1 and 3.")]
        public int CreditHours { get; set; }
    }
}
