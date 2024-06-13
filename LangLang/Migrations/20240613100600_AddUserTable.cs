using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LangLang.Migrations
{
    public partial class AddUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Languages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dictionary<int, bool>",
                columns: table => new
                {
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Dictionary<int, int>",
                columns: table => new
                {
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Education = table.Column<int>(type: "integer", nullable: true),
                    PenaltyPoints = table.Column<int>(type: "integer", nullable: true),
                    ActiveCourseId = table.Column<int>(type: "integer", nullable: true),
                    LanguagePassFail = table.Column<string>(type: "text", nullable: true),
                    AppliedExams = table.Column<List<int>>(type: "integer[]", nullable: true),
                    ExamGradeIds = table.Column<string>(type: "text", nullable: true),
                    CourseGradeIds = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Users_TeacherId",
                table: "Languages");

            migrationBuilder.DropTable(
                name: "Dictionary<int, bool>");

            migrationBuilder.DropTable(
                name: "Dictionary<int, int>");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Languages_TeacherId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Languages");
        }
    }
}
