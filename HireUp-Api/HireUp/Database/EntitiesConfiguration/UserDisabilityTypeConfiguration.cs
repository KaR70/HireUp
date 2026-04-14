using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class UserDisabilityTypeConfiguration : IEntityTypeConfiguration<UserDisabilityType>
{
    public void Configure(EntityTypeBuilder<UserDisabilityType> builder)
    {
        builder.HasKey(udt => new { udt.UserId, udt.DisabilityTypeId });
    }
}
