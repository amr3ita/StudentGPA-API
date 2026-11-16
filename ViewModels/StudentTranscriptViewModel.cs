namespace Student_GPA.ViewModels
{
    public class StudentTranscriptViewModel
    {
        public int StudentId { get; set; }
        public string StudentCode { get; set; }
        public string FullName { get; set; }
        public decimal CumulativeGPA { get; set; }
        public int TotalEarnedHours { get; set; }
        public bool IsRegistrationOpen { get; set; }
        public string? ActiveSemesterName { get; set; }

        public List<SemesterTranscriptViewModel> Semesters { get; set; } = new List<SemesterTranscriptViewModel>();
    }
}
