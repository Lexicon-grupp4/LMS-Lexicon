using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkSubmissionToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Submissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ActivityId",
                table: "Submissions",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Activities_ActivityId",
                table: "Submissions",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Activities_ActivityId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_ActivityId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Submissions");
        }
    }
}
