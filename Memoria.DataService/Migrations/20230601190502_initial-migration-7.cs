using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Memoria.DataService.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorization_Users_AuthorizedUserId",
                table: "Authorization");

            migrationBuilder.DropForeignKey(
                name: "FK_Authorization_Users_AuthorizerId",
                table: "Authorization");

            migrationBuilder.DropIndex(
                name: "IX_Authorization_AuthorizedUserId",
                table: "Authorization");

            migrationBuilder.DropIndex(
                name: "IX_Authorization_AuthorizerId",
                table: "Authorization");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizerId",
                table: "Authorization",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizedUserId",
                table: "Authorization",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AuthorizerId",
                table: "Authorization",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizedUserId",
                table: "Authorization",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_AuthorizedUserId",
                table: "Authorization",
                column: "AuthorizedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_AuthorizerId",
                table: "Authorization",
                column: "AuthorizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorization_Users_AuthorizedUserId",
                table: "Authorization",
                column: "AuthorizedUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorization_Users_AuthorizerId",
                table: "Authorization",
                column: "AuthorizerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
