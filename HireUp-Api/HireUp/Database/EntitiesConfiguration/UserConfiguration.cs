using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .OwnsMany(x => x.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.Header)
            .HasMaxLength(50);

        builder.HasMany(u => u.UserDisabilityTypes)
            .WithOne(udt => udt.User)
            .HasForeignKey(udt => udt.UserId);

        builder.HasMany(u => u.UserAccessibilityNeeds)
            .WithOne(uan => uan.User)
            .HasForeignKey(uan => uan.UserId);
    }
}
