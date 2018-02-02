using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OCR",
                table: "GoLeave",
                newName: "OutOCR");

            migrationBuilder.AddColumn<string>(
                name: "GoOCR",
                table: "GoLeave",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoOCR",
                table: "GoLeave");

            migrationBuilder.RenameColumn(
                name: "OutOCR",
                table: "GoLeave",
                newName: "OCR");
        }
    }
}
