using Student_GPA.Enum;

namespace Student_GPA.ViewModels
{
    public class GradeUpdateViewModel
    {
        public int EnrollmentId { get; set; }
        public decimal? Score { get; set; }
        public SpecialStatus Status { get; set; }
    }
}
