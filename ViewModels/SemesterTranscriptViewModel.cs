namespace Student_GPA.ViewModels
{
    public class SemesterTranscriptViewModel
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public bool IsActive { get; set; }
        public List<EnrollmentItemViewModel> Enrollments { get; set; } = new List<EnrollmentItemViewModel>();
        public int TotalSemesterHours { get; set; }
        public decimal SemesterGPA { get; set; }
    }
}
