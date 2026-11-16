using Student_GPA.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_GPA.Models
{
    public class StudentCourse
    {
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }

        public int SemesterId { get; set; }
        [ForeignKey(nameof(SemesterId))]
        public Semester Semester { get; set; }

        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100.")]
        public decimal? NumericalScore { get; set; }
        public SpecialStatus SpecialStatus { get; set; } = SpecialStatus.None;
    }
}
