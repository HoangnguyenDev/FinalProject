using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeaveOcg",
                table: "GoLeave",
                newName: "LeavePlate");

            migrationBuilder.RenameColumn(
                name: "GoOcg",
                table: "GoLeave",
                newName: "GoPlate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeavePlate",
                table: "GoLeave",
                newName: "LeaveOcg");

            migrationBuilder.RenameColumn(
                name: "GoPlate",
                table: "GoLeave",
                newName: "GoOcg");
        }
    }
}
