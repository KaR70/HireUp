using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class ExperienceLevelConfiguration : IEntityTypeConfiguration<ExperienceLevel>
{
    public void Configure(EntityTypeBuilder<ExperienceLevel> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasMany(e => e.JobListings)
            .WithOne(j => j.ExperienceLevel)
            .HasForeignKey(j => j.ExperienceLevelId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasData(
            new ExperienceLevel { Id = 1, Name = "Entry-Level" },
            new ExperienceLevel { Id = 2, Name = "Junior" },
            new ExperienceLevel { Id = 3, Name = "Mid-Level" },
            new ExperienceLevel { Id = 4, Name = "Senior" },
            new ExperienceLevel { Id = 5, Name = "Lead" }
        );
    }
}