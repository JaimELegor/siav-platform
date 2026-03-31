using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIAV.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHorarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisponibilidadesDocente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocenteId = table.Column<int>(type: "int", nullable: false),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisponibilidadesDocente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisponibilidadesDocente_UsuariosInstitucion_DocenteId",
                        column: x => x.DocenteId,
                        principalTable: "UsuariosInstitucion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Horarios_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisponibilidadesDocente_DocenteId",
                table: "DisponibilidadesDocente",
                column: "DocenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_GrupoId",
                table: "Horarios",
                column: "GrupoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisponibilidadesDocente");

            migrationBuilder.DropTable(
                name: "Horarios");
        }
    }
}
