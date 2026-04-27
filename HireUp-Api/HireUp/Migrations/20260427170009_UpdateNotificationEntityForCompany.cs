using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationEntityForCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CompanyId",
                table: "Notifications",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Companies_CompanyId",
                table: "Notifications",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Companies_CompanyId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CompanyId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Companies");
        }
    }
}
