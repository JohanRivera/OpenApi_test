using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskRegister.API.Migrations
{
    /// <inheritdoc />
    public partial class FourMigrationTaskMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProductionTimeSlips",
                newName: "SubjectUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectUserId",
                table: "ProductionTimeSlips",
                newName: "UserId");
        }
    }
}
