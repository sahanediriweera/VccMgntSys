using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VccMgntSys.Migrations
{
    public partial class addisapproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "Staff",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "Manager",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "citizens",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "Admin",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "citizens");

            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "Admin");
        }
    }
}
