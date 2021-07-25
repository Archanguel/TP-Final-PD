using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trabajo_Final___Grupo_4.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alojamiento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", nullable: false),
                    Ciudad = table.Column<string>(type: "varchar(50)", nullable: false),
                    Barrio = table.Column<string>(type: "varchar(50)", nullable: false),
                    Estrellas = table.Column<int>(nullable: false),
                    CantidadDePersonas = table.Column<int>(nullable: false),
                    Tv = table.Column<bool>(nullable: false),
                    Tipo = table.Column<string>(type: "varchar(10)", nullable: false),
                    PrecioPorPersona = table.Column<double>(nullable: false),
                    PrecioPorDia = table.Column<double>(nullable: false),
                    Habitaciones = table.Column<int>(nullable: false),
                    Banios = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alojamiento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Dni = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(80)", nullable: false),
                    Email = table.Column<string>(type: "varchar(30)", nullable: false),
                    Password = table.Column<string>(type: "varchar(200)", nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    Bloqueado = table.Column<bool>(nullable: false),
                    Intentos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaDesde = table.Column<DateTime>(nullable: false),
                    FechaHasta = table.Column<DateTime>(nullable: false),
                    AlojamientoId = table.Column<int>(nullable: true),
                    UsuarioId = table.Column<int>(nullable: true),
                    Precio = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserva_Alojamiento_AlojamientoId",
                        column: x => x.AlojamientoId,
                        principalTable: "Alojamiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reserva_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Alojamiento",
                columns: new[] { "Id", "Banios", "Barrio", "CantidadDePersonas", "Ciudad", "Codigo", "Estrellas", "Habitaciones", "PrecioPorDia", "PrecioPorPersona", "Tipo", "Tv" },
                values: new object[,]
                {
                    { 1, 0, "Recoleta", 2, "Buenos Aires", "352234", 3, 0, 0.0, 2400.0, "hotel", true },
                    { 2, 2, "Sur", 2, "Neuquen", "934120", 4, 4, 1200.0, 0.0, "cabaña", true },
                    { 3, 0, "Puerto Madero", 2, "Buenos Aires", "846445", 2, 0, 0.0, 6400.0, "hotel", true },
                    { 4, 1, "Centro", 5, "Carlos Paz", "321632", 1, 1, 2800.0, 0.0, "cabaña", true }
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Bloqueado", "Dni", "Email", "Intentos", "IsAdmin", "Nombre", "Password" },
                values: new object[,]
                {
                    { 1, false, 1234, "admin@admin.com", 0, true, "admin", "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" },
                    { 2, false, 12312312, "prueba1@gmail.com", 0, false, "prueba1", "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" },
                    { 3, true, 23423423, "prueba2@gmail.com", 0, false, "prueba2", "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" },
                    { 4, false, 34534534, "prueba3@gmail.com", 0, false, "prueba3", "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alojamiento_Codigo",
                table: "Alojamiento",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_AlojamientoId",
                table: "Reserva",
                column: "AlojamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_UsuarioId",
                table: "Reserva",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Dni",
                table: "Usuario",
                column: "Dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reserva");

            migrationBuilder.DropTable(
                name: "Alojamiento");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
