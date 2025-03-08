using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FptUOverflow.Infra.EfCore.Migrations
{
    public partial class RemoveProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetailProblemHtml",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ExpectingHtml",
                table: "Questions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetailProblemHtml",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExpectingHtml",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
