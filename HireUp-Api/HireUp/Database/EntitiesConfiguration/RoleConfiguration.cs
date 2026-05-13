using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new ApplicationRole
            {
                Id = DefaultRoles.FreelancerRoleId,
                Name = DefaultRoles.Freelancer,
                NormalizedName = DefaultRoles.Freelancer.ToUpper(),
                ConcurrencyStamp = DefaultRoles.FreelancerConcurrencyStamp,
                IsDefault = true
            },
            new ApplicationRole
            {
                Id = DefaultRoles.DisabledFreelancerRoleId,
                Name = DefaultRoles.DisabledFreelancer,
                NormalizedName = DefaultRoles.DisabledFreelancer.ToUpper(),
                ConcurrencyStamp = DefaultRoles.DisabledFreelancerConcurrencyStamp
            },
            new ApplicationRole
            {
                Id = DefaultRoles.CompanyRoleId,
                Name = DefaultRoles.Company,
                NormalizedName = DefaultRoles.Company.ToUpper(),
                ConcurrencyStamp = DefaultRoles.CompanyConcurrencyStamp
            }
        ]);
    }
}