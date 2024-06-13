using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangLang.Migrations
{
    public partial class AddLanguagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Language_LanguageId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Language",
                table: "Language");

            migrationBuilder.RenameTable(
                name: "Language",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_LanguageLevels_LanguageId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageLevels",
                table: "LanguageLevels");

            migrationBuilder.RenameTable(
                name: "LanguageLevels",
                newName: "Language");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Language",
                table: "Language",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Language_LanguageId",
                table: "Courses",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
