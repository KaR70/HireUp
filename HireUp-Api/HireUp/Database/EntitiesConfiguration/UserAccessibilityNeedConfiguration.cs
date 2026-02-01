using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class UserAccessibilityNeedConfiguration : IEntityTypeConfiguration<UserAccessibilityNeed>
{
    public void Configure(EntityTypeBuilder<UserAccessibilityNeed> builder)
    {
        builder.HasKey(uan => new { uan.UserId, uan.AccessibilityNeedId });
    }
}
