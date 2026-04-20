using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class JobRoleConfiguration : IEntityTypeConfiguration<JobRole>
{
    public void Configure(EntityTypeBuilder<JobRole> builder)
    {
        builder.HasData([
            new JobRole { Id = 1, Name = "Software Engineer" },
            new JobRole { Id = 2, Name = "Frontend Developer" },
            new JobRole { Id = 3, Name = "Backend Developer" },
            new JobRole { Id = 4, Name = "Full Stack Developer" },
            new JobRole { Id = 10, Name = "Digital Marketer" },
            new JobRole { Id = 11, Name = "Content Writer" }
        ]);
    }
}