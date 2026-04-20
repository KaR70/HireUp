using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class JobTypeConfiguration : IEntityTypeConfiguration<JobType>
{
    public void Configure(EntityTypeBuilder<JobType> builder)
    {
        builder.HasData([
                new JobType
                {
                    Id = 1,
                    Name = JobType.Contract
                },
                new JobType
                {
                    Id = 2,
                    Name = JobType.FullTime
                },
                new JobType
                {
                    Id = 3,
                    Name = JobType.PartTime
                },
                new JobType
                {
                    Id = 4,
                    Name = "Internship"
                },
                new JobType
                {
                    Id = 5,
                    Name = "Freelance"
                }
            ]
        );
    }
}