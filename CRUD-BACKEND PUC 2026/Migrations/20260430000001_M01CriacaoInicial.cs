using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CrudApp.Migrations
{
    /// <inheritdoc />
    public partial class M01CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cria a tabela de categorias
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    Id          = table.Column<int>(type: "int", nullable: false)
                                       .Annotation("SqlServer:Identity", "1, 1"),
                    Nome        = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Descricao   = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.Id);
                });

            // Cria a tabela de produtos com FK para categorias
            migrationBuilder.CreateTable(
                name: "produtos",
                columns: table => new
                {
                    Id           = table.Column<int>(type: "int", nullable: false)
                                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome         = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao    = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Preco        = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantidade   = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoriaId  = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_produtos_categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Seed das categorias padrão
            migrationBuilder.InsertData(
                table: "categorias",
                columns: new[] { "Id", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "Dispositivos eletrônicos em geral", "Eletrônicos" },
                    { 2, "Roupas e acessórios",               "Vestuário"   },
                    { 3, "Produtos alimentícios",              "Alimentação" }
                });

            // Índice na FK para agilizar buscas de produtos por categoria
            migrationBuilder.CreateIndex(
                name: "IX_produtos_CategoriaId",
                table: "produtos",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Desfaz na ordem inversa (respeita as FKs)
            migrationBuilder.DropTable(name: "produtos");
            migrationBuilder.DropTable(name: "categorias");
        }
    }
}
