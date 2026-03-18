using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Migrations
{
    /// <inheritdoc />
    public partial class FinalDatabaseSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_JobTypes_TypeId",
                table: "JobListings");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "JobListings",
                newName: "JobTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_JobListings_TypeId",
                table: "JobListings",
                newName: "IX_JobListings_JobTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_JobTypes_JobTypeId",
                table: "JobListings",
                column: "JobTypeId",
                principalTable: "JobTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_JobTypes_JobTypeId",
                table: "JobListings");

            migrationBuilder.RenameColumn(
                name: "JobTypeId",
                table: "JobListings",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_JobListings_JobTypeId",
                table: "JobListings",
                newName: "IX_JobListings_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_JobTypes_TypeId",
                table: "JobListings",
                column: "TypeId",
                principalTable: "JobTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
