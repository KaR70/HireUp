using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Migrations
{
    /// <inheritdoc />
    public partial class updateJobLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "JobListings");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "JobListings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_LocationId",
                table: "JobListings",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Locations_LocationId",
                table: "JobListings",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Locations_LocationId",
                table: "JobListings");

            migrationBuilder.DropIndex(
                name: "IX_JobListings_LocationId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "JobListings");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "JobListings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
