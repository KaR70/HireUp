using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Migrations
{
    /// <inheritdoc />
    public partial class addJobAccessibilityNeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobAccessibilityNeed",
                columns: table => new
                {
                    JobListingId = table.Column<int>(type: "int", nullable: false),
                    AccessibilityNeedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAccessibilityNeed", x => new { x.JobListingId, x.AccessibilityNeedId });
                    table.ForeignKey(
                        name: "FK_JobAccessibilityNeed_AccessibilityNeed_AccessibilityNeedId",
                        column: x => x.AccessibilityNeedId,
                        principalTable: "AccessibilityNeed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobAccessibilityNeed_JobListings_JobListingId",
                        column: x => x.JobListingId,
                        principalTable: "JobListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a72436e4b44",
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 11, 38, 54, 904, DateTimeKind.Utc).AddTicks(8174));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a741a085ba4",
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 11, 38, 54, 904, DateTimeKind.Utc).AddTicks(8529));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a761414137c",
                column: "CreatedAt",
                value: new DateTime(2026, 6, 15, 11, 38, 54, 904, DateTimeKind.Utc).AddTicks(8711));

            migrationBuilder.CreateIndex(
                name: "IX_JobAccessibilityNeed_AccessibilityNeedId",
                table: "JobAccessibilityNeed",
                column: "AccessibilityNeedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobAccessibilityNeed");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a72436e4b44",
                column: "CreatedAt",
                value: new DateTime(2026, 5, 16, 16, 3, 13, 204, DateTimeKind.Utc).AddTicks(5005));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a741a085ba4",
                column: "CreatedAt",
                value: new DateTime(2026, 5, 16, 16, 3, 13, 204, DateTimeKind.Utc).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a761414137c",
                column: "CreatedAt",
                value: new DateTime(2026, 5, 16, 16, 3, 13, 204, DateTimeKind.Utc).AddTicks(5452));
        }
    }
}
