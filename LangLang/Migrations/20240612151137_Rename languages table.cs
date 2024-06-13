using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangLang.Migrations
{
    public partial class Renamelanguagestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_LanguageLevels_LanguageId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageLevels",
                table: "LanguageLevels");

            migrationBuilder.RenameTable(
                name: "LanguageLevels",
                newName: "Languages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Languages_LanguageId",
                table: "Courses",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Languages_LanguageId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.RenameTable(
                name: "Languages",
                newName: "LanguageLevels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageLevels",
                table: "LanguageLevels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_LanguageLevels_LanguageId",
                table: "Courses",
                column: "LanguageId",
                principalTable: "LanguageLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
