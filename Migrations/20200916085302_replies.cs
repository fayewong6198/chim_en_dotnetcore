using Microsoft.EntityFrameworkCore.Migrations;

namespace Chim_En_DOTNET.Migrations
{
    public partial class replies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatingCount",
                table: "Product",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalRating",
                table: "Product",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingCount",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TotalRating",
                table: "Product");
        }
    }
}
