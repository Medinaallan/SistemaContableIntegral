using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaComunidad.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroReciboAportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FechaAporte",
                table: "Aportes",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "NumeroRecibo",
                table: "Aportes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroRecibo",
                table: "Aportes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAporte",
                table: "Aportes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }
    }
}
