using System.ComponentModel.DataAnnotations;

namespace Student_GPA.ViewModels
{
    public class StudentFormViewModel
    {
        public int Id { get; set; }
        public string StudentCode { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        [StringLength(150)]
        public string FullName { get; set; }
    }
}
