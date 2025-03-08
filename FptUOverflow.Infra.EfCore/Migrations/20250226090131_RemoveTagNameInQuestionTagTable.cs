using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FptUOverflow.Infra.EfCore.Migrations
{
    public partial class RemoveTagNameInQuestionTagTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagName",
                table: "QuestionTags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TagName",
                table: "QuestionTags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
