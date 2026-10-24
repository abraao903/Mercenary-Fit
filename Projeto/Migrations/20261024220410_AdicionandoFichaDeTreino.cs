using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoFichaDeTreino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichaDeTreino_Alunos_AlunoId",
                table: "FichaDeTreino");

            migrationBuilder.DropForeignKey(
                name: "FK_FichaDeTreino_Instrutores_InstrutorId",
                table: "FichaDeTreino");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FichaDeTreino",
                table: "FichaDeTreino");

            migrationBuilder.RenameTable(
                name: "FichaDeTreino",
                newName: "FichaDeTreinos");

            migrationBuilder.RenameIndex(
                name: "IX_FichaDeTreino_InstrutorId",
                table: "FichaDeTreinos",
                newName: "IX_FichaDeTreinos_InstrutorId");

            migrationBuilder.RenameIndex(
                name: "IX_FichaDeTreino_AlunoId",
                table: "FichaDeTreinos",
                newName: "IX_FichaDeTreinos_AlunoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FichaDeTreinos",
                table: "FichaDeTreinos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FichaDeTreinos_Alunos_AlunoId",
                table: "FichaDeTreinos",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FichaDeTreinos_Instrutores_InstrutorId",
                table: "FichaDeTreinos",
                column: "InstrutorId",
                principalTable: "Instrutores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichaDeTreinos_Alunos_AlunoId",
                table: "FichaDeTreinos");

            migrationBuilder.DropForeignKey(
                name: "FK_FichaDeTreinos_Instrutores_InstrutorId",
                table: "FichaDeTreinos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FichaDeTreinos",
                table: "FichaDeTreinos");

            migrationBuilder.RenameTable(
                name: "FichaDeTreinos",
                newName: "FichaDeTreino");

            migrationBuilder.RenameIndex(
                name: "IX_FichaDeTreinos_InstrutorId",
                table: "FichaDeTreino",
                newName: "IX_FichaDeTreino_InstrutorId");

            migrationBuilder.RenameIndex(
                name: "IX_FichaDeTreinos_AlunoId",
                table: "FichaDeTreino",
                newName: "IX_FichaDeTreino_AlunoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FichaDeTreino",
                table: "FichaDeTreino",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FichaDeTreino_Alunos_AlunoId",
                table: "FichaDeTreino",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FichaDeTreino_Instrutores_InstrutorId",
                table: "FichaDeTreino",
                column: "InstrutorId",
                principalTable: "Instrutores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
