using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HireUp.Migrations
{
    /// <inheritdoc />
    public partial class seedDisabilityAndAccessibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccessibilityNeed",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Requires company internal tools, software systems, and portals to be fully compatible with software like JAWS, NVDA, or VoiceOver.", "Screen-Reader Compatible Software" },
                    { 2, "Requires digital documentation, corporate handbooks, and operational dashboards to support dark themes and font size scalability.", "High-Contrast UI & Document Formats" },
                    { 3, "Prefers business communication, daily updates, and feedback to take place via Slack, Teams, or email rather than audio/video calling.", "Asynchronous Text-First Communication" },
                    { 4, "Requires all company-wide, all-hands meetings or team syncs to provide real-time automated or manual closed captions.", "Live Meeting Closed-Captioning" },
                    { 5, "Requires developer environments, corporate web tools, and command interfaces to be fully operational without forcing standard mouse use.", "Keyboard-Only Digital Navigation" },
                    { 6, "For hybrid roles: Requires step-free physical premises or company-provided ergonomic accessories (e.g., split keyboards, vertical mice, voice-to-text software).", "Accessible Physical Workspace / Ergonomic Equipment" },
                    { 7, "Requires clear, unambiguous written briefs and ticket documentation rather than loose verbal instructions to optimize workflow execution.", "Explicit, Written Project Requirements" },
                    { 8, "Requires structural freedom to organize daily focus hours and take micro-breaks to effectively navigate focus and energy variations.", "Flexible Task Breaks & Core Hour Buffers" },
                    { 9, "Requires the allowance to attend routine medical follow-ups, therapy, or checkups during business hours without formal disciplinary tracking.", "Flexible Medical Appointment Leave" }
                });

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
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 16, 16, 3, 13, 204, DateTimeKind.Utc).AddTicks(5388), "AQAAAAIAAYagAAAAEIZKwT+8mydB5fsv8Z/4fPhwJWQWExqwoYln75Yn8aZDGKqX8F5XCMwtgAcm3WSszQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a761414137c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 16, 16, 3, 13, 204, DateTimeKind.Utc).AddTicks(5452), "AQAAAAIAAYagAAAAEIZKwT+8mydB5fsv8Z/4fPhwJWQWExqwoYln75Yn8aZDGKqX8F5XCMwtgAcm3WSszQ==" });

            migrationBuilder.InsertData(
                table: "DisabilityTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Candidates who are blind, have low vision, or experience color-blindness and utilize assistive visual tools or screen modifications.", "Visual Disability" },
                    { 2, "Candidates who are deaf or hard-of-hearing and rely on written communication channels or captioning frameworks.", "Hearing or Auditory Disability" },
                    { 3, "Candidates with limited fine motor skills, repetitive strain injuries, or mobility variations requiring ergonomic or alternative physical/digital access.", "Physical or Mobility Disability" },
                    { 4, "Candidates with ADHD, Autism, Dyslexia, or processing variations who thrive with structured workflows, clear communication, or specialized environments.", "Neurodivergence & Cognitive Variations" },
                    { 5, "Candidates managing long-term health conditions (e.g., autoimmune, chronic pain) requiring energy management or medical schedule flexibility.", "Chronic Illness or Invisible Disability" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessibilityNeed",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccessibilityNeed",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AccessibilityNeed",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AccessibilityNeed",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AccessibilityNeed",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AccessibilityNeed",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AccessibilityNeed",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AccessibilityNeed",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AccessibilityNeed",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "DisabilityTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DisabilityTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DisabilityTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DisabilityTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DisabilityTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a72436e4b44",
                column: "CreatedAt",
                value: new DateTime(2026, 5, 13, 6, 19, 5, 315, DateTimeKind.Utc).AddTicks(8724));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a741a085ba4",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 13, 6, 19, 5, 413, DateTimeKind.Utc).AddTicks(9344), "AQAAAAIAAYagAAAAEEoDTalruqkbMHqtCWBFysDGslHxgkU+wVfE61qD6z/U9Evds2y7cK9uHN32pQQ52Q==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019e1f8f-4e4e-7ef2-a8e4-1a761414137c",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 13, 6, 19, 5, 511, DateTimeKind.Utc).AddTicks(9263), "AQAAAAIAAYagAAAAEN7wOfp7cRjkhmYvnLfeN7+MOXAHrXJfM8KLYlEd8zJantZ0dWHdL5WDOFPHoB9SEw==" });
        }
    }
}
