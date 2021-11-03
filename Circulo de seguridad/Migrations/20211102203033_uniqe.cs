using Microsoft.EntityFrameworkCore.Migrations;

namespace Circulo_de_seguridad.Migrations
{
    public partial class uniqe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email_NickName",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NickName",
                table: "Usuarios",
                column: "NickName",
                unique: true,
                filter: "[NickName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_NickName",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email_NickName",
                table: "Usuarios",
                columns: new[] { "Email", "NickName" },
                unique: true,
                filter: "[Email] IS NOT NULL AND [NickName] IS NOT NULL");
        }
    }
}
