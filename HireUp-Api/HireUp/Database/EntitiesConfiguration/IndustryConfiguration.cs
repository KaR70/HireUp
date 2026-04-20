using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class IndustryConfiguration : IEntityTypeConfiguration<Industry>
{
    public void Configure(EntityTypeBuilder<Industry> builder)
    {
        builder.HasData([
            new Industry { Id = 1, Name = "Technology & Software" },
            new Industry { Id = 2, Name = "Design & Creative" },
            new Industry { Id = 3, Name = "Sales & Marketing" },
            new Industry { Id = 4, Name = "Writing & Translation" },
            new Industry { Id = 5, Name = "Finance & Accounting" },
            new Industry { Id = 6, Name = "Legal & Consulting" },
            new Industry { Id = 7, Name = "Engineering & Architecture" },
            new Industry { Id = 8, Name = "Customer Support" }
        ]);
    }
}