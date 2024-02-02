using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeJudgeSystemWebApplication.Migrations.File
{
    /// <inheritdoc />
    public partial class AddGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Files");
        }
    }
}
