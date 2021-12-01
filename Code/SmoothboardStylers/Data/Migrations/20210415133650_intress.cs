using Microsoft.EntityFrameworkCore.Migrations;

namespace Smoothboardstylers.Data.Migrations
{
    public partial class intress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Surfboard",
                table: "Interesse");

            migrationBuilder.CreateIndex(
                name: "IX_Interesse_SurfboardId",
                table: "Interesse",
                column: "SurfboardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interesse_Surfboards_SurfboardId",
                table: "Interesse",
                column: "SurfboardId",
                principalTable: "Surfboards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interesse_Surfboards_SurfboardId",
                table: "Interesse");

            migrationBuilder.DropIndex(
                name: "IX_Interesse_SurfboardId",
                table: "Interesse");

            migrationBuilder.AddColumn<int>(
                name: "Surfboard",
                table: "Interesse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
