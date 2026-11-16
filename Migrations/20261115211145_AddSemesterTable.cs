using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_GPA.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "AcademicSemester",
                table: "StudentCourses");

            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "StudentCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_SemesterId",
                table: "StudentCourses",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId_CourseId",
                table: "StudentCourses",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Semesters_SemesterId",
                table: "StudentCourses",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Semesters_SemesterId",
                table: "StudentCourses");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_SemesterId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_StudentId_CourseId",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "StudentCourses");

            migrationBuilder.AddColumn<string>(
                name: "AcademicSemester",
                table: "StudentCourses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses",
                column: "StudentId");
        }
    }
}
