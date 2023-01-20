using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asp.NetStudentManagementSystem.Migrations
{
    public partial class GradeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Grade",
                table: "Attendances",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Attendances");
        }
    }
}
