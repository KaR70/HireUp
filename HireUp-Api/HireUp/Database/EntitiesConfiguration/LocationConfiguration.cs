using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.Property(l => l.Country)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(l => l.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(l => l.Users)
            .WithOne(u => u.Location)
            .HasForeignKey(u => u.LocationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict); 
        
        builder.HasIndex(l => new { l.Country, l.City }).IsUnique();
    }
}