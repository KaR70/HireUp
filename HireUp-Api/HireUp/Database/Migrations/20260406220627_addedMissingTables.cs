using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireUp.Database.Migrations
{
    /// <inheritdoc />
    public partial class addedMissingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "JobListings",
                newName: "JobTypeId");

            migrationBuilder.CreateTable(
                name: "JobRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserJobCategoryPreferences",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobCategoryPreferences", x => new { x.UserId, x.JobCategoryId });
                    table.ForeignKey(
                        name: "FK_UserJobCategoryPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJobCategoryPreferences_JobCategories_JobCategoryId",
                        column: x => x.JobCategoryId,
                        principalTable: "JobCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLocationPreferences",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocationPreferences", x => new { x.UserId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_UserLocationPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLocationPreferences_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "UserJobTypePreferences",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobTypeId = table.Column<int>(type: "int", nullable: false),
                    JobRoleId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    OfficeTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobTypePreferences", x => new { x.UserId, x.JobTypeId });
                    table.ForeignKey(
                        name: "FK_UserJobTypePreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJobTypePreferences_JobRoles_JobRoleId",
                        column: x => x.JobRoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserJobTypePreferences_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJobTypePreferences_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserJobTypePreferences_OfficeTypes_OfficeTypeId",
                        column: x => x.OfficeTypeId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserOfficeTypePreferences",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfficeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOfficeTypePreferences", x => new { x.UserId, x.OfficeTypeId });
                    table.ForeignKey(
                        name: "FK_UserOfficeTypePreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOfficeTypePreferences_OfficeTypes_OfficeTypeId",
                        column: x => x.OfficeTypeId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_JobTypeId",
                table: "JobListings",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobCategoryPreferences_JobCategoryId",
                table: "UserJobCategoryPreferences",
                column: "JobCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobRolePreferences_JobRoleId",
                table: "UserJobRolePreferences",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobTypePreferences_JobRoleId",
                table: "UserJobTypePreferences",
                column: "JobRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobTypePreferences_JobTypeId",
                table: "UserJobTypePreferences",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobTypePreferences_LocationId",
                table: "UserJobTypePreferences",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobTypePreferences_OfficeTypeId",
                table: "UserJobTypePreferences",
                column: "OfficeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocationPreferences_LocationId",
                table: "UserLocationPreferences",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOfficeTypePreferences_OfficeTypeId",
                table: "UserOfficeTypePreferences",
                column: "OfficeTypeId");

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

            migrationBuilder.DropTable(
                name: "UserJobCategoryPreferences");

            migrationBuilder.DropTable(
                name: "UserJobRolePreferences");

            migrationBuilder.DropTable(
                name: "UserJobTypePreferences");

            migrationBuilder.DropTable(
                name: "UserLocationPreferences");

            migrationBuilder.DropTable(
                name: "UserOfficeTypePreferences");

            migrationBuilder.DropTable(
                name: "JobRoles");

            migrationBuilder.DropTable(
                name: "JobTypes");

            migrationBuilder.DropTable(
                name: "OfficeTypes");

            migrationBuilder.DropIndex(
                name: "IX_JobListings_JobTypeId",
                table: "JobListings");

            migrationBuilder.RenameColumn(
                name: "JobTypeId",
                table: "JobListings",
                newName: "Type");
        }
    }
}
