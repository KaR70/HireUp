using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class JobCategoryConfiguration : IEntityTypeConfiguration<JobCategory>
{
    public void Configure(EntityTypeBuilder<JobCategory> builder)
    {
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.HasData(
            new JobCategory { Id = 1, Name = "IT" },
            new JobCategory { Id = 2, Name = "Design" },
            new JobCategory { Id = 3, Name = "Marketing" }
        );
    }
}