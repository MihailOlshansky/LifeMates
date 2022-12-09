using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeMates.Storage.Migrations
{
    public partial class AddedReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Chat",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 6, 11, 47, 13, 1, DateTimeKind.Utc).AddTicks(7788),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 5, 13, 20, 54, 971, DateTimeKind.Utc).AddTicks(3889));

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ComplainerId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Chat",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 5, 13, 20, 54, 971, DateTimeKind.Utc).AddTicks(3889),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 6, 11, 47, 13, 1, DateTimeKind.Utc).AddTicks(7788));
        }
    }
}
