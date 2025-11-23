using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProgressTrackingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PercentCompleted",
                table: "CourseProgresses",
                newName: "LessonsCompleted");

            migrationBuilder.AddColumn<int>(
                name: "TimeWatchedSeconds",
                table: "CourseLessonProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeWatchedSeconds",
                table: "CourseLessonProgresses");

            migrationBuilder.RenameColumn(
                name: "LessonsCompleted",
                table: "CourseProgresses",
                newName: "PercentCompleted");
        }
    }
}
