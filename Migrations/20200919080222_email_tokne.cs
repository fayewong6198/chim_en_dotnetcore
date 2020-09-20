using Microsoft.EntityFrameworkCore.Migrations;

namespace Chim_En_DOTNET.Migrations
{
    public partial class email_tokne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "Review",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailToken",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "EmailToken",
                table: "AspNetUsers");
        }
    }
}
