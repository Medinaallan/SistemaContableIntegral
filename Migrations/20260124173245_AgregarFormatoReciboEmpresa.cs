using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaComunidad.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFormatoReciboEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormatoRecibo",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormatoRecibo",
                table: "Empresas");
        }
    }
}
