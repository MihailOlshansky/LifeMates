using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeMates.Storage.Migrations
{
    public partial class FixInterestFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInterest_Interest_UserId",
                table: "UserInterest");

            migrationBuilder.CreateIndex(
                name: "IX_UserInterest_InterestId",
                table: "UserInterest",
                column: "InterestId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterest_Interest_InterestId",
                table: "UserInterest",
                column: "InterestId",
                principalTable: "Interest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInterest_Interest_InterestId",
                table: "UserInterest");

            migrationBuilder.DropIndex(
                name: "IX_UserInterest_InterestId",
                table: "UserInterest");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterest_Interest_UserId",
                table: "UserInterest",
                column: "UserId",
                principalTable: "Interest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
