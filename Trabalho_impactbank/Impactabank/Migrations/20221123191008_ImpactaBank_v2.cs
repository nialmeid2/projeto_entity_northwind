using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Impactabank.Migrations
{
    public partial class ImpactaBank_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Clientes");
        }
    }
}
