using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLessons_Courses_CourseId",
                table: "CourseLessons");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CourseLessons",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseLessons_CourseId",
                table: "CourseLessons",
                newName: "IX_CourseLessons_ModuleId");

            migrationBuilder.AddColumn<int>(
                name: "LessonsLength",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CourseModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    User = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseModules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseModules_CourseId",
                table: "CourseModules",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLessons_CourseModules_ModuleId",
                table: "CourseLessons",
                column: "ModuleId",
                principalTable: "CourseModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLessons_CourseModules_ModuleId",
                table: "CourseLessons");

            migrationBuilder.DropTable(
                name: "CourseModules");

            migrationBuilder.DropColumn(
                name: "LessonsLength",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "CourseLessons",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseLessons_ModuleId",
                table: "CourseLessons",
                newName: "IX_CourseLessons_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLessons_Courses_CourseId",
                table: "CourseLessons",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
