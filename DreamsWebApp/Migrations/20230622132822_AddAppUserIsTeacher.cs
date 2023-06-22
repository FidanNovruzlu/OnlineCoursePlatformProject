using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamsWebApp.Migrations
{
    public partial class AddAppUserIsTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTeacher",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTeacher",
                table: "AspNetUsers");
        }
    }
}
