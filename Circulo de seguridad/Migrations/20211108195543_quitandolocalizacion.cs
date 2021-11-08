using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Circulo_de_seguridad.Migrations
{
    public partial class quitandolocalizacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "localizacionUsuarios");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "localizacionUsuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoordenadaX = table.Column<float>(type: "real", nullable: false),
                    CoordenadaY = table.Column<float>(type: "real", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlAvatar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localizacionUsuarios", x => x.Id);
                });
        }
    }
}
