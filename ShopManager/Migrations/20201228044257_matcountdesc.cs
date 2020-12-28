using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopManager.Migrations
{
    public partial class matcountdesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MaterialCounts",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MaterialCounts");
        }
    }
}
