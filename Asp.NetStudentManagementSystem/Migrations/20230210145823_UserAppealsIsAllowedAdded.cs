using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asp.NetStudentManagementSystem.Migrations
{
    public partial class UserAppealsIsAllowedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAllowed",
                table: "StudentAppeals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllowed",
                table: "StudentAppeals");
        }
    }
}
