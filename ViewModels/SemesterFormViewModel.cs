using System.ComponentModel.DataAnnotations;

namespace Student_GPA.ViewModels
{
    public class SemesterFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Semester name is required.")]
        [StringLength(100)]
        public string SemesterName { get; set; }

        public bool IsActive { get; set; }
    }
}
