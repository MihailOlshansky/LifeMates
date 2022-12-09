using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeMates.Storage.Migrations
{
    public partial class AddedLikesAndDislikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 19, 18, 15, 15, 240, DateTimeKind.Utc).AddTicks(8873));

            migrationBuilder.CreateTable(
                name: "UserDislikes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DislikedUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDislikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDislikes_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLikes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LikedUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLikes_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDislikes_UserId",
                table: "UserDislikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikes_UserId",
                table: "UserLikes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDislikes");

            migrationBuilder.DropTable(
                name: "UserLikes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "User");
        }
    }
}
