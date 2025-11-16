using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIncomes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "wallet_category",
                table: "wallet",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "currency",
                table: "wallet",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "operation_type",
                table: "expense",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "wallet_id",
                table: "expense",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_expense_wallet_id",
                table: "expense",
                column: "wallet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_expense_wallet_wallet_id",
                table: "expense",
                column: "wallet_id",
                principalTable: "wallet",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_expense_wallet_wallet_id",
                table: "expense");

            migrationBuilder.DropIndex(
                name: "IX_expense_wallet_id",
                table: "expense");

            migrationBuilder.DropColumn(
                name: "operation_type",
                table: "expense");

            migrationBuilder.DropColumn(
                name: "wallet_id",
                table: "expense");

            migrationBuilder.AlterColumn<int>(
                name: "wallet_category",
                table: "wallet",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "currency",
                table: "wallet",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);
        }
    }
}
