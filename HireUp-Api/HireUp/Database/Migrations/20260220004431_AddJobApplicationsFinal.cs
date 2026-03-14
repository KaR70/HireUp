using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddJobApplicationsFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_JobSeekerId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobListings_JobListingId",
                table: "Applications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "JobApplications");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_JobSeekerId",
                table: "JobApplications",
                newName: "IX_JobApplications_JobSeekerId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_JobListingId",
                table: "JobApplications",
                newName: "IX_JobApplications_JobListingId");

            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_JobSeekerId",
                table: "JobApplications",
                column: "JobSeekerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_JobListings_JobListingId",
                table: "JobApplications",
                column: "JobListingId",
                principalTable: "JobListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_JobSeekerId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_JobListings_JobListingId",
                table: "JobApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "JobApplications");

            migrationBuilder.RenameTable(
                name: "JobApplications",
                newName: "Applications");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_JobSeekerId",
                table: "Applications",
                newName: "IX_Applications_JobSeekerId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_JobListingId",
                table: "Applications",
                newName: "IX_Applications_JobListingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_JobSeekerId",
                table: "Applications",
                column: "JobSeekerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobListings_JobListingId",
                table: "Applications",
                column: "JobListingId",
                principalTable: "JobListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
