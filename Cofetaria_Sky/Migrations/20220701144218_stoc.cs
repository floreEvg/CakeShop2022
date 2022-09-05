using Microsoft.EntityFrameworkCore.Migrations;

namespace Cofetaria_Sky.Migrations
{
    public partial class stoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Stock",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");
        }
    }
}
