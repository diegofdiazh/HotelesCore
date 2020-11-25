using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelesCore.Migrations
{
    public partial class hoteles1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {           

            migrationBuilder.CreateTable(
                name: "ReservaHotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoHotel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaHotels", x => x.Id);
                });
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            

            migrationBuilder.DropTable(
                name: "ReservaHotels");           
        }
    }
}
