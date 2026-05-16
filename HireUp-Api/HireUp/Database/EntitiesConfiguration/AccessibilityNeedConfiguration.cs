using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class AccessibilityNeedConfiguration : IEntityTypeConfiguration<AccessibilityNeed>
{
    public void Configure(EntityTypeBuilder<AccessibilityNeed> builder)
    {
        builder.HasKey(an => an.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(250);

        builder.HasMany(an => an.UserAccessibilityNeeds)
            .WithOne(uan => uan.AccessibilityNeed)
            .HasForeignKey(uan => uan.AccessibilityNeedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData([
            new AccessibilityNeed
            {
                Id = 1,
                Name = "Screen-Reader Compatible Software",
                Description =
                    "Requires company internal tools, software systems, and portals to be fully compatible with software like JAWS, NVDA, or VoiceOver."
            },
            new AccessibilityNeed
            {
                Id = 2,
                Name = "High-Contrast UI & Document Formats",
                Description =
                    "Requires digital documentation, corporate handbooks, and operational dashboards to support dark themes and font size scalability."
            },
            new AccessibilityNeed
            {
                Id = 3,
                Name = "Asynchronous Text-First Communication",
                Description =
                    "Prefers business communication, daily updates, and feedback to take place via Slack, Teams, or email rather than audio/video calling."
            },
            new AccessibilityNeed
            {
                Id = 4,
                Name = "Live Meeting Closed-Captioning",
                Description =
                    "Requires all company-wide, all-hands meetings or team syncs to provide real-time automated or manual closed captions."
            },
            new AccessibilityNeed
            {
                Id = 5,
                Name = "Keyboard-Only Digital Navigation",
                Description =
                    "Requires developer environments, corporate web tools, and command interfaces to be fully operational without forcing standard mouse use."
            },
            new AccessibilityNeed
            {
                Id = 6,
                Name = "Accessible Physical Workspace / Ergonomic Equipment",
                Description =
                    "For hybrid roles: Requires step-free physical premises or company-provided ergonomic accessories (e.g., split keyboards, vertical mice, voice-to-text software)."
            },
            new AccessibilityNeed
            {
                Id = 7,
                Name = "Explicit, Written Project Requirements",
                Description =
                    "Requires clear, unambiguous written briefs and ticket documentation rather than loose verbal instructions to optimize workflow execution."
            },
            new AccessibilityNeed
            {
                Id = 8,
                Name = "Flexible Task Breaks & Core Hour Buffers",
                Description =
                    "Requires structural freedom to organize daily focus hours and take micro-breaks to effectively navigate focus and energy variations."
            },
            new AccessibilityNeed
            {
                Id = 9,
                Name = "Flexible Medical Appointment Leave",
                Description =
                    "Requires the allowance to attend routine medical follow-ups, therapy, or checkups during business hours without formal disciplinary tracking."
            }
        ]);
    }
}
