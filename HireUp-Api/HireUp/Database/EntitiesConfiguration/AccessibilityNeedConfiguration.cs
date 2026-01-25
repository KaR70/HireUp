using Microsoft.EntityFrameworkCore;
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
    }
}
