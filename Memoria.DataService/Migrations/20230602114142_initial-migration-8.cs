using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Memoria.DataService.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Users_LabelerId",
                table: "Labels");

            migrationBuilder.DropIndex(
                name: "IX_Labels_LabelerId",
                table: "Labels");

            migrationBuilder.AlterColumn<string>(
                name: "LabelerId",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LabelerId",
                table: "Labels",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_LabelerId",
                table: "Labels",
                column: "LabelerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Users_LabelerId",
                table: "Labels",
                column: "LabelerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
