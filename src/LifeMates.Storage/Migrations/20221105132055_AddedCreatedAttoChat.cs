using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeMates.Storage.Migrations
{
    public partial class AddedCreatedAttoChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Chat",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 5, 13, 20, 54, 971, DateTimeKind.Utc).AddTicks(3889));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Chat");
        }
    }
}
