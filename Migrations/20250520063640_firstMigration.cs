using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_comerce.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LINEAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LINEA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FECHA_CREADO = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LINEAS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MARCAS_PRODUCTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MARCA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FECHA_CREADO = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MARCAS_PRODUCTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTOS_SAT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLAVE_PROD = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DESCRIPCION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FECHA_CREADO = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTOS_SAT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UNIDADES_SAT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLAVE_UNIDAD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UNIDAD_SAT = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    FECHA_CREADO = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNIDADES_SAT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoSatId = table.Column<int>(type: "int", nullable: true),
                    PREFIJO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PRODUCTO = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ACUMULADOR = table.Column<bool>(type: "bit", nullable: true),
                    PRODUCTO_ID_ACUMULADOR = table.Column<int>(type: "int", nullable: true),
                    LINEA_ID = table.Column<int>(type: "int", nullable: true),
                    MARCA_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTOS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUCTOS_LINEAS_LINEA_ID",
                        column: x => x.LINEA_ID,
                        principalTable: "LINEAS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PRODUCTOS_MARCAS_PRODUCTO_MARCA_ID",
                        column: x => x.MARCA_ID,
                        principalTable: "MARCAS_PRODUCTO",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PRODUCTOS_PRODUCTOS_SAT_ProductoSatId",
                        column: x => x.ProductoSatId,
                        principalTable: "PRODUCTOS_SAT",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EMPAQUES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMPAQUE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CONTENIDO = table.Column<double>(type: "float", nullable: true),
                    SINCRONIZADO = table.Column<bool>(type: "bit", nullable: true),
                    CODIGO_EMPAQUE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FECHA_CREADO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UNIDAD_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPAQUES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EMPAQUES_UNIDADES_SAT_UNIDAD_ID",
                        column: x => x.UNIDAD_ID,
                        principalTable: "UNIDADES_SAT",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTO_EMPAQUE",
                columns: table => new
                {
                    PRODUCTO_ID = table.Column<int>(type: "int", nullable: false),
                    EMPAQUE_ID = table.Column<int>(type: "int", nullable: false),
                    CODIGO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PCOMPRA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PVENTA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DESCUENTO = table.Column<double>(type: "float", nullable: true),
                    ACTIVO = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTO_EMPAQUE", x => new { x.PRODUCTO_ID, x.EMPAQUE_ID });
                    table.ForeignKey(
                        name: "FK_PRODUCTO_EMPAQUE_EMPAQUES_EMPAQUE_ID",
                        column: x => x.EMPAQUE_ID,
                        principalTable: "EMPAQUES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRODUCTO_EMPAQUE_PRODUCTOS_PRODUCTO_ID",
                        column: x => x.PRODUCTO_ID,
                        principalTable: "PRODUCTOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EMPAQUES_UNIDAD_ID",
                table: "EMPAQUES",
                column: "UNIDAD_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTO_EMPAQUE_EMPAQUE_ID",
                table: "PRODUCTO_EMPAQUE",
                column: "EMPAQUE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTOS_LINEA_ID",
                table: "PRODUCTOS",
                column: "LINEA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTOS_MARCA_ID",
                table: "PRODUCTOS",
                column: "MARCA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTOS_ProductoSatId",
                table: "PRODUCTOS",
                column: "ProductoSatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRODUCTO_EMPAQUE");

            migrationBuilder.DropTable(
                name: "EMPAQUES");

            migrationBuilder.DropTable(
                name: "PRODUCTOS");

            migrationBuilder.DropTable(
                name: "UNIDADES_SAT");

            migrationBuilder.DropTable(
                name: "LINEAS");

            migrationBuilder.DropTable(
                name: "MARCAS_PRODUCTO");

            migrationBuilder.DropTable(
                name: "PRODUCTOS_SAT");
        }
    }
}
