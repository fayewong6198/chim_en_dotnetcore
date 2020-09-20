using Microsoft.EntityFrameworkCore.Migrations;

namespace Chim_En_DOTNET.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevicedId",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "Payment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "DevicedId",
                table: "Payment",
                type: "text",
                nullable: true);
        }
    }
}
