using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamsWebApp.Migrations
{
    public partial class AddColumnBasketItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BasketItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "BasketItems");
        }
    }
}
