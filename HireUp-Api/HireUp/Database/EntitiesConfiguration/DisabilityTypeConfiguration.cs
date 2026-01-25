using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class DisabilityTypeConfiguration : IEntityTypeConfiguration<DisabilityType>
{
    public void Configure(EntityTypeBuilder<DisabilityType> builder)
    {
        builder.HasKey(dt => dt.Id);

        builder.Property(dt => dt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(dt => dt.Description)
            .HasMaxLength(250);

        builder.HasMany(dt => dt.UserDisabilityTypes)
            .WithOne(udt => udt.DisabilityType)
            .HasForeignKey(udt => udt.DisabilityTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
