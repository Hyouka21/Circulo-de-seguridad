using Microsoft.EntityFrameworkCore.Migrations;

namespace Circulo_de_seguridad.Migrations
{
    public partial class admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Grupos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_AdminId",
                table: "Grupos",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupos_Usuarios_AdminId",
                table: "Grupos",
                column: "AdminId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupos_Usuarios_AdminId",
                table: "Grupos");

            migrationBuilder.DropIndex(
                name: "IX_Grupos_AdminId",
                table: "Grupos");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Grupos");
        }
    }
}
