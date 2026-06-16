using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class JobAccessibilityNeedConfiguration : IEntityTypeConfiguration<JobAccessibilityNeed>
{
    public void Configure(EntityTypeBuilder<JobAccessibilityNeed> builder)
    {
        builder.HasKey(jan => new { jan.JobListingId, jan.AccessibilityNeedId });

        builder.HasOne(jan => jan.JobListing)
            .WithMany(j => j.JobAccessibilityNeeds)
            .HasForeignKey(jan => jan.JobListingId);
        
        builder.HasOne(jan => jan.AccessibilityNeed)
            .WithMany(an => an.JobAccessibilityNeeds)
            .HasForeignKey(jan => jan.AccessibilityNeedId);
    }
}