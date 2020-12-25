using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopManager.Migrations
{
    public partial class materialdeletepolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialCounts_Materials_MaterialId",
                table: "MaterialCounts");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialCounts_Materials_MaterialId",
                table: "MaterialCounts",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialCounts_Materials_MaterialId",
                table: "MaterialCounts");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialCounts_Materials_MaterialId",
                table: "MaterialCounts",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
