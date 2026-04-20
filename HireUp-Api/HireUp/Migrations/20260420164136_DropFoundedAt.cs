using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Migrations
{
    /// <inheritdoc />
    public partial class DropFoundedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Industry_IndustryId",
                table: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Industry",
                table: "Industry");

            migrationBuilder.DropColumn(
                name: "FoundedAt",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Industry",
                newName: "Industries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Industries",
                table: "Industries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Industries_IndustryId",
                table: "Companies",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Industries_IndustryId",
                table: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Industries",
                table: "Industries");

            migrationBuilder.RenameTable(
                name: "Industries",
                newName: "Industry");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FoundedAt",
                table: "Companies",
                type: "date",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Industry",
                table: "Industry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Industry_IndustryId",
                table: "Companies",
                column: "IndustryId",
                principalTable: "Industry",
                principalColumn: "Id");
        }
    }
}
