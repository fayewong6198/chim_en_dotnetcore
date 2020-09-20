using Microsoft.EntityFrameworkCore.Migrations;

namespace Chim_En_DOTNET.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "PaymentUserDetail",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUserDetail_PaymentId",
                table: "PaymentUserDetail",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentUserDetail_Payment_PaymentId",
                table: "PaymentUserDetail",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentUserDetail_Payment_PaymentId",
                table: "PaymentUserDetail");

            migrationBuilder.DropIndex(
                name: "IX_PaymentUserDetail_PaymentId",
                table: "PaymentUserDetail");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "PaymentUserDetail");
        }
    }
}
