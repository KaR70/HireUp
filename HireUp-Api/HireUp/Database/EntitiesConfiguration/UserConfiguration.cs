using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireUp.Database.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .OwnsMany(x => x.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.Header)
            .HasMaxLength(50);

        builder.HasMany(u => u.UserDisabilityTypes)
            .WithOne(udt => udt.User)
            .HasForeignKey(udt => udt.UserId);

        builder.HasMany(u => u.UserAccessibilityNeeds)
            .WithOne(uan => uan.User)
            .HasForeignKey(uan => uan.UserId);
        
        //var passwordHasher = new PasswordHasher<ApplicationUser>();

        builder.HasData([
            new ApplicationUser
            {
                Id = DefaultUsers.FreelancerId,
                FirstName = "Freelancer",
                LastName = "1",
                UserName = DefaultUsers.FreelancerEmail,
                NormalizedUserName = DefaultUsers.FreelancerEmail.ToUpper(),
                Email = DefaultUsers.FreelancerEmail,
                NormalizedEmail = DefaultUsers.FreelancerEmail.ToUpper(),
                SecurityStamp = DefaultUsers.FreelancerSecurityStamp,
                ConcurrencyStamp = DefaultUsers.FreelancerConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEIZKwT+8mydB5fsv8Z/4fPhwJWQWExqwoYln75Yn8aZDGKqX8F5XCMwtgAcm3WSszQ=="
            },
            new ApplicationUser
            {
                Id = DefaultUsers.DisabledFreelancerId,
                FirstName = "Freelancer",
                LastName = "Disabled",
                UserName = DefaultUsers.DisabledFreelancerEmail,
                NormalizedUserName = DefaultUsers.DisabledFreelancerEmail.ToUpper(),
                Email = DefaultUsers.DisabledFreelancerEmail,
                NormalizedEmail = DefaultUsers.DisabledFreelancerEmail.ToUpper(),
                SecurityStamp = DefaultUsers.DisabledFreelancerSecurityStamp,
                ConcurrencyStamp = DefaultUsers.DisabledFreelancerConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEIZKwT+8mydB5fsv8Z/4fPhwJWQWExqwoYln75Yn8aZDGKqX8F5XCMwtgAcm3WSszQ=="
            },
            new ApplicationUser
            {
                Id = DefaultUsers.CompanyOwnerId,
                FirstName = "Company",
                LastName = "Owner",
                UserName = DefaultUsers.CompanyOwnerEmail,
                NormalizedUserName = DefaultUsers.CompanyOwnerEmail.ToUpper(),
                Email = DefaultUsers.CompanyOwnerEmail,
                NormalizedEmail = DefaultUsers.CompanyOwnerEmail.ToUpper(),
                SecurityStamp = DefaultUsers.CompanyOwnerSecurityStamp,
                ConcurrencyStamp = DefaultUsers.CompanyOwnerConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEIZKwT+8mydB5fsv8Z/4fPhwJWQWExqwoYln75Yn8aZDGKqX8F5XCMwtgAcm3WSszQ=="
            }
        ]);
    }
}
