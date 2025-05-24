using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api_comerce.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lineas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Linea = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lineas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarcasProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcasProducto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductosSat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaveProd = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosSat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesSat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaveUnidad = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UnidadSat = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesSat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountsTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoSatId = table.Column<int>(type: "int", nullable: true),
                    Prefijo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NombreProducto = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DescripcionBreve = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Acumulador = table.Column<bool>(type: "bit", nullable: true),
                    ProductoIdAcumulador = table.Column<int>(type: "int", nullable: true),
                    LineaId = table.Column<int>(type: "int", nullable: true),
                    MarcaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Lineas_LineaId",
                        column: x => x.LineaId,
                        principalTable: "Lineas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Productos_MarcasProducto_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "MarcasProducto",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Productos_ProductosSat_ProductoSatId",
                        column: x => x.ProductoSatId,
                        principalTable: "ProductosSat",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Empaques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Empaque = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Contenido = table.Column<double>(type: "float", nullable: true),
                    Sincronizado = table.Column<bool>(type: "bit", nullable: true),
                    CodigoEmpaque = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnidadId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empaques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empaques_UnidadesSat_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "UnidadesSat",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductoEmpaque",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    EmpaqueId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Descuento = table.Column<double>(type: "float", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoEmpaque", x => new { x.ProductoId, x.EmpaqueId });
                    table.ForeignKey(
                        name: "FK_ProductoEmpaque_Empaques_EmpaqueId",
                        column: x => x.EmpaqueId,
                        principalTable: "Empaques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductoEmpaque_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountsTypes",
                columns: new[] { "Id", "AccountType", "IsActive" },
                values: new object[,]
                {
                    { 1, "Manager", true },
                    { 2, "Asesor", true },
                    { 3, "Cliente", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId",
                table: "Accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Empaques_UnidadId",
                table: "Empaques",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoEmpaque_EmpaqueId",
                table: "ProductoEmpaque",
                column: "EmpaqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_LineaId",
                table: "Productos",
                column: "LineaId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_MarcaId",
                table: "Productos",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_ProductoSatId",
                table: "Productos",
                column: "ProductoSatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "ProductoEmpaque");

            migrationBuilder.DropTable(
                name: "AccountsTypes");

            migrationBuilder.DropTable(
                name: "Empaques");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "UnidadesSat");

            migrationBuilder.DropTable(
                name: "Lineas");

            migrationBuilder.DropTable(
                name: "MarcasProducto");

            migrationBuilder.DropTable(
                name: "ProductosSat");
        }
    }
}
