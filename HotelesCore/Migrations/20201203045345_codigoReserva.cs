using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelesCore.Migrations
{
    public partial class codigoReserva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoReserva",
                table: "ReservaHoteles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoReserva",
                table: "ReservaHoteles");
        }
    }
}
