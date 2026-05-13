using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData([
            new IdentityUserRole<string>
            {
                UserId = DefaultUsers.FreelancerId,
                RoleId = DefaultRoles.FreelancerRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = DefaultUsers.DisabledFreelancerId,
                RoleId = DefaultRoles.DisabledFreelancerRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = DefaultUsers.CompanyOwnerId,
                RoleId = DefaultRoles.CompanyRoleId
            }
        ]);
    }
}