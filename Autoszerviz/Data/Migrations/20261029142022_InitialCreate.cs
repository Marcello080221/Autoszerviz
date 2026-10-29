using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoszerviz.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ugyfelek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nev = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ugyfelek", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Javitasok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UgyfelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rendszam = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Leiras = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Koltseg = table.Column<decimal>(type: "TEXT", nullable: true),
                    Allapot = table.Column<int>(type: "INTEGER", nullable: false),
                    FelvetelDatuma = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Javitasok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Javitasok_Ugyfelek_UgyfelId",
                        column: x => x.UgyfelId,
                        principalTable: "Ugyfelek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Javitasok_UgyfelId",
                table: "Javitasok",
                column: "UgyfelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Javitasok");

            migrationBuilder.DropTable(
                name: "Ugyfelek");
        }
    }
}
