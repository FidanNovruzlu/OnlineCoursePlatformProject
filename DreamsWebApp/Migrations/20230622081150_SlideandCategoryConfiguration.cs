using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamsWebApp.Migrations
{
    public partial class SlideandCategoryConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slides_Categories_CategoryId",
                table: "Slides");

            migrationBuilder.DropIndex(
                name: "IX_Slides_CategoryId",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Slides");

            migrationBuilder.CreateIndex(
                name: "IX_Slides_CatagoryId",
                table: "Slides",
                column: "CatagoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Categories_CatagoryId",
                table: "Slides",
                column: "CatagoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slides_Categories_CatagoryId",
                table: "Slides");

            migrationBuilder.DropIndex(
                name: "IX_Slides_CatagoryId",
                table: "Slides");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Slides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Slides_CategoryId",
                table: "Slides",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Categories_CategoryId",
                table: "Slides",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
