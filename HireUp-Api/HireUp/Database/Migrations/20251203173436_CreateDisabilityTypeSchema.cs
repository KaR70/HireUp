using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateDisabilityTypeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisabilityType",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "DisabilityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisabilityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDisabilityTypes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisabilityTypeId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDisabilityTypes", x => new { x.UserId, x.DisabilityTypeId });
                    table.ForeignKey(
                        name: "FK_UserDisabilityTypes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDisabilityTypes_DisabilityTypes_DisabilityTypeId",
                        column: x => x.DisabilityTypeId,
                        principalTable: "DisabilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDisabilityTypes_DisabilityTypeId",
                table: "UserDisabilityTypes",
                column: "DisabilityTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDisabilityTypes");

            migrationBuilder.DropTable(
                name: "DisabilityTypes");

            migrationBuilder.AddColumn<string>(
                name: "DisabilityType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
