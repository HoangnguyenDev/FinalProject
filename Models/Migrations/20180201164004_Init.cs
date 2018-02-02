using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    CreateDT = table.Column<DateTime>(nullable: true),
                    DateIdentityCard = table.Column<DateTime>(nullable: true),
                    DateofBirth = table.Column<DateTime>(nullable: true),
                    FirstMidName = table.Column<string>(nullable: true),
                    ImageID = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    MobilePhone = table.Column<string>(nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StudentID = table.Column<int>(nullable: false),
                    UniversityID = table.Column<int>(nullable: false),
                    UpdateDT = table.Column<DateTime>(nullable: true),
                    WhereIdentityCard = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GoLeave",
                columns: table => new
                {
                    ImageID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GoAvatar = table.Column<string>(nullable: true),
                    GoDT = table.Column<DateTime>(nullable: true),
                    GoFull = table.Column<string>(nullable: true),
                    GoOcg = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    LeaveDT = table.Column<DateTime>(nullable: true),
                    LeaveFull = table.Column<string>(nullable: true),
                    LeaveOcg = table.Column<string>(nullable: true),
                    OCR = table.Column<string>(nullable: true),
                    OwnerID = table.Column<long>(nullable: false),
                    leaveAvatar = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoLeave", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_GoLeave_Member_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Member",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoLeave_OwnerID",
                table: "GoLeave",
                column: "OwnerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoLeave");

            migrationBuilder.DropTable(
                name: "Member");
        }
    }
}
