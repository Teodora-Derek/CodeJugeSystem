using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeJudgeSystemWebApplication.Migrations.Assignment
{
    /// <inheritdoc />
    public partial class UpdateAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedInput",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "ExpectedOutput",
                table: "Assignments",
                newName: "ExpectedInputAndOutputPairs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpectedInputAndOutputPairs",
                table: "Assignments",
                newName: "ExpectedOutput");

            migrationBuilder.AddColumn<string>(
                name: "ExpectedInput",
                table: "Assignments",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
