using Microsoft.EntityFrameworkCore.Migrations;

namespace NyscIdentify.Common.Infrastructure.Migrations
{
    public partial class Resources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalPath",
                table: "Resources",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalPath",
                table: "Resources");
        }
    }
}
