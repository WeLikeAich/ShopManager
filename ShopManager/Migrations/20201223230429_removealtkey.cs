using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopManager.Migrations
{
    public partial class removealtkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_FriendlyName",
                table: "Materials");

            migrationBuilder.AlterColumn<string>(
                name: "FriendlyName",
                table: "Materials",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FriendlyName",
                table: "Materials",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_FriendlyName",
                table: "Materials",
                column: "FriendlyName");
        }
    }
}
