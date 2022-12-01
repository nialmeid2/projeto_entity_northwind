using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Impactabank.Migrations
{
    public partial class DataNoExtrato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataHora",
                table: "ExtratoOperacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHora",
                table: "ExtratoOperacoes");
        }
    }
}
