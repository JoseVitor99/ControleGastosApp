using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleGastoApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDescricaoTamanho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Transacoes",
                type: "TEXT",
                maxLength: 400,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Transacoes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 400);
        }
    }
}
