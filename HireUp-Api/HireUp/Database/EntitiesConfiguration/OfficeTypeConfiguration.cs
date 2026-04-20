using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class OfficeTypeConfiguration : IEntityTypeConfiguration<OfficeType>
{
    public void Configure(EntityTypeBuilder<OfficeType> builder)
    {
        builder.HasData([
            new OfficeType { Id = 1, Name = "On-Site" },
            new OfficeType { Id = 2, Name = "Remote" },
            new OfficeType { Id = 3, Name = "Hybrid" }
        ]);
    }
}