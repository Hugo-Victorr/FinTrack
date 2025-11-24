using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Database.Migrations
{
    /// <inheritdoc />
    public partial class MoveOperationTypeToExpenseCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "operation_type",
                table: "expense");

            migrationBuilder.AddColumn<int>(
                name: "operation_type",
                table: "expense_category",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "operation_type",
                table: "expense_category");

            migrationBuilder.AddColumn<int>(
                name: "operation_type",
                table: "expense",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
