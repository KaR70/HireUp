using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HireUp.Migrations
{
    /// <inheritdoc />
    public partial class updateRolesTablesAndSeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019e1f8f-4e4e-7ef2-a8e4-1a78e9919201", "019e1f8f-4e4e-7ef2-a8e4-1a798004f0bb", true, false, "Freelancer", "FREELANCER" },
                    { "019e1f8f-4e4e-7ef2-a8e4-1a7afe2dcc4f", "019e1f8f-4e4e-7ef2-a8e4-1a7bcb3cc53a", false, false, "DisabledFreelancer", "DISABLEDFREELANCER" },
                    { "019e1fab-e4a0-7f8e-b3ac-b99c68309f25", "019e1fab-e4a0-7f8e-b3ac-b99db5cbc5bc", false, false, "Company", "COMPANY" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bio", "Birthday", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "Gender", "Header", "IsActive", "JobRoleId", "LastName", "LocationId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PasswordResetCode", "PasswordResetCodeExpiry", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "019e1f8f-4e4e-7ef2-a8e4-1a72436e4b44", 0, null, null, "019e1f8f-4e4e-7ef2-a8e4-1a73be063a7f", new DateTime(2026, 5, 13, 6, 19, 5, 315, DateTimeKind.Utc).AddTicks(8724), "Freelancer@Hire-Up.com", true, "Freelancer", null, null, true, null, "1", null, false, null, "FREELANCER@HIRE-UP.COM", "FREELANCER@HIRE-UP.COM", "AQAAAAIAAYagAAAAEIZKwT+8mydB5fsv8Z/4fPhwJWQWExqwoYln75Yn8aZDGKqX8F5XCMwtgAcm3WSszQ==", null, null, null, false, null, "D2CA77D6E24940FF9BF8408FA0AC6D62", false, "Freelancer@Hire-Up.com" },
                    { "019e1f8f-4e4e-7ef2-a8e4-1a741a085ba4", 0, null, null, "019e1f8f-4e4e-7ef2-a8e4-1a75706084e3", new DateTime(2026, 5, 13, 6, 19, 5, 413, DateTimeKind.Utc).AddTicks(9344), "Disabled-Freelancer@Hire-Up.com", true, "Freelancer", null, null, true, null, "Disabled", null, false, null, "DISABLED-FREELANCER@HIRE-UP.COM", "DISABLED-FREELANCER@HIRE-UP.COM", "AQAAAAIAAYagAAAAEEoDTalruqkbMHqtCWBFysDGslHxgkU+wVfE61qD6z/U9Evds2y7cK9uHN32pQQ52Q==", null, null, null, false, null, "746B204025174F4FBF897E51CF10C8AE", false, "Disabled-Freelancer@Hire-Up.com" },
                    { "019e1f8f-4e4e-7ef2-a8e4-1a761414137c", 0, null, null, "019e1f8f-4e4e-7ef2-a8e4-1a777d50816d", new DateTime(2026, 5, 13, 6, 19, 5, 511, DateTimeKind.Utc).AddTicks(9263), "Company@Hire-Up.com", true, "Company", null, null, true, null, "Owner", null, false, null, "COMPANY@HIRE-UP.COM", "COMPANY@HIRE-UP.COM", "AQAAAAIAAYagAAAAEN7wOfp7cRjkhmYvnLfeN7+MOXAHrXJfM8KLYlEd8zJantZ0dWHdL5WDOFPHoB9SEw==", null, null, null, false, null, "6CF4145034F9493E8B7540DD827F65CF", false, "Company@Hire-Up.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "019e1f8f-4e4e-7ef2-a8e4-1a78e9919201", "019e1f8f-4e4e-7ef2-a8e4-1a72436e4b44" },
                    { "019e1f8f-4e4e-7ef2-a8e4-1a7afe2dcc4f", "019e1f8f-4e4e-7ef2-a8e4-1a741a085ba4" },
                    { "019e1fab-e4a0-7f8e-b3ac-b99c68309f25", "019e1f8f-4e4e-7ef2-a8e4-1a761414137c" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Description", "FoundedYear", "IndustryId", "LinkedIn", "LocationId", "Logo", "Name", "UserId", "Website" },
                values: new object[] { 999999, null, null, null, null, null, null, "Hire Up", "019e1f8f-4e4e-7ef2-a8e4-1a761414137c", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019e1f8f-4e4e-7ef2-a8e4-1a78e9919201", "019e1f8f-4e4e-7ef2-a8e4-1a72436e4b44" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019e1f8f-4e4e-7ef2-a8e4-1a7afe2dcc4f", "019e1f8f-4e4e-7ef2-a8e4-1a741a085ba4" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019e1fab-e4a0-7f8e-b3ac-b99c68309f25", "019e1f8f-4e4e-7ef2-a8e4-1a761414137c" });

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 999999);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a78e9919201");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a7afe2dcc4f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019e1fab-e4a0-7f8e-b3ac-b99c68309f25");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a72436e4b44");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a741a085ba4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a761414137c");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetRoles");
        }
    }
}
