using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfficeTypeId",
                table: "JobListings",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a72436e4b44",
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 8, 13, 28, 282, DateTimeKind.Utc).AddTicks(249));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a741a085ba4",
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 8, 13, 28, 282, DateTimeKind.Utc).AddTicks(604));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a761414137c",
                column: "CreatedAt",
                value: new DateTime(2026, 6, 16, 8, 13, 28, 282, DateTimeKind.Utc).AddTicks(772));

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_OfficeTypeId",
                table: "JobListings",
                column: "OfficeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_OfficeTypes_OfficeTypeId",
                table: "JobListings",
                column: "OfficeTypeId",
                principalTable: "OfficeTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_OfficeTypes_OfficeTypeId",
                table: "JobListings");

            migrationBuilder.DropIndex(
                name: "IX_JobListings_OfficeTypeId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "OfficeTypeId",
                table: "JobListings");

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
        }
    }
}
