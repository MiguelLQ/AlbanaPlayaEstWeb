using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace albanaPlayaEst.Migrations
{
    /// <inheritdoc />
    public partial class Mi16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "Pagos");

            migrationBuilder.RenameColumn(
                name: "FechaInicio",
                table: "Pagos",
                newName: "FechaPag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaPag",
                table: "Pagos",
                newName: "FechaInicio");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "Pagos",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
