using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessibilityNeedSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDisabilityTypes_DisabilityTypes_DisabilityTypeId",
                table: "UserDisabilityTypes");

            migrationBuilder.DropColumn(
                name: "AccessibilityNeeds",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AccessibilityNeed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessibilityNeed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccessibilityNeed",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessibilityNeedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccessibilityNeed", x => new { x.UserId, x.AccessibilityNeedId });
                    table.ForeignKey(
                        name: "FK_UserAccessibilityNeed_AccessibilityNeed_AccessibilityNeedId",
                        column: x => x.AccessibilityNeedId,
                        principalTable: "AccessibilityNeed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAccessibilityNeed_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccessibilityNeed_AccessibilityNeedId",
                table: "UserAccessibilityNeed",
                column: "AccessibilityNeedId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDisabilityTypes_DisabilityTypes_DisabilityTypeId",
                table: "UserDisabilityTypes",
                column: "DisabilityTypeId",
                principalTable: "DisabilityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDisabilityTypes_DisabilityTypes_DisabilityTypeId",
                table: "UserDisabilityTypes");

            migrationBuilder.DropTable(
                name: "UserAccessibilityNeed");

            migrationBuilder.DropTable(
                name: "AccessibilityNeed");

            migrationBuilder.AddColumn<string>(
                name: "AccessibilityNeeds",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDisabilityTypes_DisabilityTypes_DisabilityTypeId",
                table: "UserDisabilityTypes",
                column: "DisabilityTypeId",
                principalTable: "DisabilityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
