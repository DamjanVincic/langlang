using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangLang.Migrations
{
    public partial class Addlistofteacherstolanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Users_TeacherId",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Languages_TeacherId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Languages");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfReviews",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalRating",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LanguageTeacher",
                columns: table => new
                {
                    QualificationsId = table.Column<int>(type: "integer", nullable: false),
                    TeachersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTeacher", x => new { x.QualificationsId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_LanguageTeacher_Languages_QualificationsId",
                        column: x => x.QualificationsId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageTeacher_Users_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageTeacher_TeachersId",
                table: "LanguageTeacher",
                column: "TeachersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageTeacher");

            migrationBuilder.DropColumn(
                name: "NumberOfReviews",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalRating",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Languages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_TeacherId",
                table: "Languages",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Users_TeacherId",
                table: "Languages",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
