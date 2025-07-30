using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace FinanceHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserConfirm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Embedding",
                table: "Transactions");

            migrationBuilder.AddColumn<bool>(
                name: "UserApproved",
                table: "Transactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserApproved",
                table: "Transactions");

            migrationBuilder.AddColumn<Vector>(
                name: "Embedding",
                table: "Transactions",
                type: "vector",
                nullable: true);
        }
    }
}
