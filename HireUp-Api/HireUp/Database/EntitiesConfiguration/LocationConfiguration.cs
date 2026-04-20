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
        
        builder.HasData([
            new Location { Id = 1, Country = "Egypt", City = "Cairo" },
            new Location { Id = 2, Country = "Egypt", City = "Alexandria" },
            new Location { Id = 3, Country = "Saudi Arabia", City = "Riyadh" },
            new Location { Id = 4, Country = "Saudi Arabia", City = "Jeddah" },
            new Location { Id = 5, Country = "United Arab Emirates", City = "Dubai" },
            new Location { Id = 6, Country = "United Arab Emirates", City = "Abu Dhabi" },
            new Location { Id = 7, Country = "Jordan", City = "Amman" },
            new Location { Id = 8, Country = "Qatar", City = "Doha" },
            new Location { Id = 9, Country = "Kuwait", City = "Kuwait City" },
            new Location { Id = 10, Country = "Lebanon", City = "Beirut" },
            new Location { Id = 11, Country = "Morocco", City = "Casablanca" },
            new Location { Id = 12, Country = "Tunisia", City = "Tunis" },
            new Location { Id = 13, Country = "Bahrain", City = "Manama" },
            new Location { Id = 14, Country = "United States", City = "New York" },
            new Location { Id = 15, Country = "United Kingdom", City = "London" }
        ]);
    }
}