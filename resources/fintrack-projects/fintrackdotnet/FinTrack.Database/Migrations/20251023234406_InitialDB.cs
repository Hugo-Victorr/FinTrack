using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:currency_type", "usd,brl,eur,gbp,jpy")
                .Annotation("Npgsql:Enum:wallet_type", "cash,digital_wallet,credit_card,savings_account");

            migrationBuilder.CreateTable(
                name: "expense_category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wallet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    currency = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    wallet_category = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "expense",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    expense_category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    expense_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense", x => x.id);
                    table.ForeignKey(
                        name: "FK_expense_expense_category_expense_category_id",
                        column: x => x.expense_category_id,
                        principalTable: "expense_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_expense_created_at",
                table: "expense",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_expense_deleted_at",
                table: "expense",
                column: "deleted_at");

            migrationBuilder.CreateIndex(
                name: "IX_expense_expense_category_id",
                table: "expense",
                column: "expense_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_expense_updated_at",
                table: "expense",
                column: "updated_at");

            migrationBuilder.CreateIndex(
                name: "IX_expense_user_id",
                table: "expense",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_expense_category_created_at",
                table: "expense_category",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_expense_category_deleted_at",
                table: "expense_category",
                column: "deleted_at");

            migrationBuilder.CreateIndex(
                name: "IX_expense_category_updated_at",
                table: "expense_category",
                column: "updated_at");

            migrationBuilder.CreateIndex(
                name: "IX_expense_category_user_id",
                table: "expense_category",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_wallet_created_at",
                table: "wallet",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_wallet_deleted_at",
                table: "wallet",
                column: "deleted_at");

            migrationBuilder.CreateIndex(
                name: "IX_wallet_updated_at",
                table: "wallet",
                column: "updated_at");

            migrationBuilder.CreateIndex(
                name: "IX_wallet_user_id",
                table: "wallet",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expense");

            migrationBuilder.DropTable(
                name: "wallet");

            migrationBuilder.DropTable(
                name: "expense_category");
        }
    }
}
