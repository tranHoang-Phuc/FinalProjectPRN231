using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FptUOverflow.Infra.EfCore.Migrations
{
    public partial class AddFKForTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Users_CreatedUserId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_CreatedUserId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Tags");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Tags",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreatedBy",
                table: "Tags",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Users_CreatedBy",
                table: "Tags",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Users_CreatedBy",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_CreatedBy",
                table: "Tags");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserId",
                table: "Tags",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreatedUserId",
                table: "Tags",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Users_CreatedUserId",
                table: "Tags",
                column: "CreatedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
