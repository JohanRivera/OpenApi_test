using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskRegister.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialTaskRegisterAPIMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionTimeSlips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssignedTo = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    TaskDescription = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Time = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateLog = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionTimeSlips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
                    Project = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionTimeSlips_Subject",
                table: "ProductionTimeSlips",
                column: "Subject",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Project",
                table: "Projects",
                column: "Project",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionTimeSlips");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
