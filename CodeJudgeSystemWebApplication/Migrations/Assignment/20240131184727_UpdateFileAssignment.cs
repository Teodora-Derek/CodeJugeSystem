using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeJudgeSystemWebApplication.Migrations.Assignment
{
    /// <inheritdoc />
    public partial class UpdateFileAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Files",
                type: "int",
                nullable: false
            );

            migrationBuilder.CreateIndex(
                name: "IX_Files_AssignmentId",
                table: "Files",
                column: "AssignmentId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Assignments_AssignmentId",
                table: "Files",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                  name: "FK_Files_Assignments_AssignmentId",
                  table: "Files"
            );

            migrationBuilder.DropIndex(
                name: "IX_Files_AssignmentId",
                table: "Files"
            );

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Files"
            );
        }
    }
}
