using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DataAccess;

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180201170940_Init2")]
    partial class Init2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataAccess.GoLeave", b =>
                {
                    b.Property<long>("ImageID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GoAvatar");

                    b.Property<DateTime?>("GoDT");

                    b.Property<string>("GoFull");

                    b.Property<string>("GoOcg");

                    b.Property<bool>("IsDelete");

                    b.Property<DateTime?>("LeaveDT");

                    b.Property<string>("LeaveFull");

                    b.Property<string>("LeaveOcg");

                    b.Property<string>("OCR");

                    b.Property<long>("OwnerID");

                    b.Property<string>("leaveAvatar");

                    b.HasKey("ImageID");

                    b.HasIndex("OwnerID");

                    b.ToTable("GoLeave");
                });

            modelBuilder.Entity("DataAccess.Member", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<DateTime?>("CreateDT");

                    b.Property<DateTime?>("DateIdentityCard");

                    b.Property<DateTime?>("DateofBirth");

                    b.Property<string>("FirstMidName");

                    b.Property<long?>("ImageID");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<string>("MobilePhone");

                    b.Property<int>("Sex");

                    b.Property<int>("Status");

                    b.Property<int>("StudentID");

                    b.Property<int>("UniversityID");

                    b.Property<DateTime?>("UpdateDT");

                    b.Property<string>("WhereIdentityCard");

                    b.HasKey("ID");

                    b.ToTable("Member");
                });

            modelBuilder.Entity("DataAccess.GoLeave", b =>
                {
                    b.HasOne("DataAccess.Member", "Owner")
                        .WithMany("ListHistory")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
