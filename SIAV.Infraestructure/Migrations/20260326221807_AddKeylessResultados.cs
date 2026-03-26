using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIAV.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKeylessResultados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CursosPorAlumno",
                columns: table => new
                {
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    GrupoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CursoId = table.Column<int>(type: "int", nullable: false),
                    CursoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CursosPorAlumno");
        }
    }
}
