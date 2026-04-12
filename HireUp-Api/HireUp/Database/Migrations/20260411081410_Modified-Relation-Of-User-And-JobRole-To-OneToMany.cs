using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Database.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedRelationOfUserAndJobRoleToOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserJobTypePreferences_JobRoles_JobRoleId",
                table: "UserJobTypePreferences");

            migrationBuilder.DropTable(
                name: "UserJobRolePreferences");

            migrationBuilder.DropIndex(
                name: "IX_UserJobTypePreferences_JobRoleId",
                table: "UserJobTypePreferences");

            migrationBuilder.DropColumn(
                name: "JobRoleId",
                table: "UserJobTypePreferences");

            migrationBuilder.AddColumn<int>(
                name: "JobRoleId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobRoleId",
                table: "AspNetUsers",
                column: "JobRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobRoles_JobRoleId",
                table: "AspNetUsers",
                column: "JobRoleId",
                principalTable: "JobRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobRoles_JobRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JobRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobRoleId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "JobRoleId",
                table: "UserJobTypePreferences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserJobRolePreferences",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobRolePreferences", x => new { x.UserId, x.JobRoleId });
                    table.ForeignKey(
                        name: "FK_UserJobRolePreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJobRolePreferences_JobRoles_JobRoleId",
                        column: x => x.JobRoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserJobTypePreferences_JobRoleId",
                table: "UserJobTypePreferences",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobRolePreferences_JobRoleId",
                table: "UserJobRolePreferences",
                column: "JobRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobTypePreferences_JobRoles_JobRoleId",
                table: "UserJobTypePreferences",
                column: "JobRoleId",
                principalTable: "JobRoles",
                principalColumn: "Id");
        }
    }
}
