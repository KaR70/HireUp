using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class JobListingConfiguration : IEntityTypeConfiguration<JobListing>
{
    public void Configure(EntityTypeBuilder<JobListing> builder)
    {
        builder.Property(j => j.Salary)
            .HasPrecision(18, 2);

        builder.Property(j => j.IsFeatured)
            .HasDefaultValue(false);

        builder.Property(j => j.ViewCount)
            .HasDefaultValue(0);

        builder.HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}