using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Impactabank.Migrations
{
    public partial class MudandoMunicipioPraObjeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Municipio",
                table: "Clientes");

            migrationBuilder.AddColumn<int>(
                name: "MunicipioId",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_MunicipioId",
                table: "Clientes",
                column: "MunicipioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Municipios_MunicipioId",
                table: "Clientes",
                column: "MunicipioId",
                principalTable: "Municipios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Municipios_MunicipioId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_MunicipioId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "MunicipioId",
                table: "Clientes");

            migrationBuilder.AddColumn<string>(
                name: "Municipio",
                table: "Clientes",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");
        }
    }
}
