using Microsoft.EntityFrameworkCore;

namespace Student_GPA.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<GradingScale> GradingScales { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.StudentCode)
                .IsUnique();

            modelBuilder.Entity<Course>()
                .HasIndex(c => c.CourseCode)
                .IsUnique();

            modelBuilder.Entity<StudentCourse>()
                .HasIndex(sc => new { sc.StudentId, sc.CourseId })
                .IsUnique();


            modelBuilder.Entity<GradingScale>().HasData(
                new GradingScale { Id = 1, FromGrade = 96, ToGrade = 101, Points = 4.0m, Grade = "A+" },
                new GradingScale { Id = 2, FromGrade = 92, ToGrade = 96, Points = 3.7m, Grade = "A" },
                new GradingScale { Id = 3, FromGrade = 88, ToGrade = 92, Points = 3.4m, Grade = "A-" },
                new GradingScale { Id = 4, FromGrade = 84, ToGrade = 88, Points = 3.2m, Grade = "B+" },
                new GradingScale { Id = 5, FromGrade = 80, ToGrade = 84, Points = 3.0m, Grade = "B" },
                new GradingScale { Id = 6, FromGrade = 76, ToGrade = 80, Points = 2.8m, Grade = "B-" },
                new GradingScale { Id = 7, FromGrade = 72, ToGrade = 76, Points = 2.6m, Grade = "C+" },
                new GradingScale { Id = 8, FromGrade = 68, ToGrade = 72, Points = 2.4m, Grade = "C" },
                new GradingScale { Id = 9, FromGrade = 64, ToGrade = 68, Points = 2.2m, Grade = "C-" },
                new GradingScale { Id = 10, FromGrade = 60, ToGrade = 64, Points = 2.0m, Grade = "D+" },
                new GradingScale { Id = 11, FromGrade = 55, ToGrade = 60, Points = 1.5m, Grade = "D" },
                new GradingScale { Id = 12, FromGrade = 50, ToGrade = 55, Points = 1.0m, Grade = "D-" },
                new GradingScale { Id = 13, FromGrade = 0, ToGrade = 50, Points = 0.0m, Grade = "F" }
            );

        }

    }
}
