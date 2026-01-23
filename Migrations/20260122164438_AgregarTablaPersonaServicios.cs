using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaComunidad.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaPersonaServicios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonaServicios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    ServicioId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaFin = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false),
                    CostoPersonalizado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UltimoPeriodoCobrado = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaServicios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonaServicios_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonaServicios_Servicios_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonaServicios_EstaActivo",
                table: "PersonaServicios",
                column: "EstaActivo");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaServicios_PersonaId",
                table: "PersonaServicios",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaServicios_PersonaId_ServicioId",
                table: "PersonaServicios",
                columns: new[] { "PersonaId", "ServicioId" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonaServicios_ServicioId",
                table: "PersonaServicios",
                column: "ServicioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonaServicios");
        }
    }
}
