using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NyscIdentify.Common.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    OtherNames = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    FileNo = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: false),
                    StateOfOrigin = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Qualification = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Rank = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
