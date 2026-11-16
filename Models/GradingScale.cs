using System.ComponentModel.DataAnnotations;

namespace Student_GPA.Models
{
    public class GradingScale
    {
        public int Id { get; set; }
        [Required]
        public decimal FromGrade { get; set; }
        [Required]
        public decimal ToGrade { get; set; }
        [Required]
        public decimal Points { get; set; }
        [Required]
        [StringLength(5)]
        public string Grade { get; set; }

    }
}
