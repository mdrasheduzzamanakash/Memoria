﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Memoria.DataService.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "emailVerificationUniqueToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isEmailConfirmed",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "emailVerificationUniqueToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "isEmailConfirmed",
                table: "Users");
        }
    }
}
