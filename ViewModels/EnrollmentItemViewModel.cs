namespace Student_GPA.ViewModels
{
    public class EnrollmentItemViewModel
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int CreditHours { get; set; }
        public decimal? NumericalScore { get; set; }
        public string LetterGrade { get; set; } = "-";
        public decimal Points { get; set; }
        public string Status { get; set; } = "None";
    }
}
