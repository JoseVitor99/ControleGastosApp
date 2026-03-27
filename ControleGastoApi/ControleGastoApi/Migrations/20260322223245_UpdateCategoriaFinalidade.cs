using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleGastoApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoriaFinalidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Finalidade",
                table: "Categorias",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finalidade",
                table: "Categorias");
        }
    }
}
