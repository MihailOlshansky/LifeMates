using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeMates.Storage.Migrations
{
    public partial class UpdateChatUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "ChatUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "ChatUser");
        }
    }
}
