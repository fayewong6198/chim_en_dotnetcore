using Microsoft.EntityFrameworkCore.Migrations;

namespace Chim_En_DOTNET.Migrations
{
    public partial class addstaff2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isSuperUser",
                table: "AspNetUsers",
                newName: "IsSuperUser");

            migrationBuilder.RenameColumn(
                name: "isStaff",
                table: "AspNetUsers",
                newName: "IsStaff");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSuperUser",
                table: "AspNetUsers",
                newName: "isSuperUser");

            migrationBuilder.RenameColumn(
                name: "IsStaff",
                table: "AspNetUsers",
                newName: "isStaff");
        }
    }
}
