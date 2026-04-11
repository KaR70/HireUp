using HireUp.Database;
using HireUp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DataSeeder
{
    public static async Task SeedAllAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        await SeedLookupsAsync(context);
        await SeedRolesAndUsersAsync(userManager, roleManager, context);
        await SeedFeaturesAsync(context);
    }

    private static async Task SeedLookupsAsync(ApplicationDbContext context)
    {
        if (!await context.Skills.AnyAsync())
        {
            await context.Skills.AddRangeAsync(new List<Skill>
            {
                new() { Name = "C#", Description = "Backend development", Category = SkillCategory.Technical },
                new() { Name = "React", Description = "Frontend development", Category = SkillCategory.Technical },
                new() { Name = "UI/UX Design", Description = "Design skills", Category = SkillCategory.Professional }
            });
        }

        if (!await context.JobTypes.AnyAsync())
        {
            await context.JobTypes.AddRangeAsync(new List<JobType>
            {
                new() { Name = "Full-time" },
                new() { Name = "Part-time" },
                new() { Name = "Freelance" },
                new() { Name = "Internship" }
            });
        }

        if (!await context.ExperienceLevels.AnyAsync())
        {
            await context.ExperienceLevels.AddRangeAsync(new List<ExperienceLevel>
            {
                new() { Name = "Entry-Level" },
                new() { Name = "Junior" },
                new() { Name = "Mid-Level" },
                new() { Name = "Senior" },
                new() { Name = "Lead" }
            });
        }

        if (!await context.JobCategories.AnyAsync())
        {
            await context.JobCategories.AddRangeAsync(new List<JobCategory>
            {
                new() { Name = "IT" },
                new() { Name = "Design" },
                new() { Name = "Marketing" }
            });
        }

        if (!await context.Companies.AnyAsync())
        {
            await context.Companies.AddRangeAsync(new List<Company>
    {
        new() {
            Name = "Facebook",
            Description = "Social media and technology company.",
            Logo = "http://localhost:8089/Logos/Facebook.png"
        },
        new() {
            Name = "Google",
            Description = "Search engine and cloud computing services.",
            Logo = "http://localhost:8089/Logos/Google.png"
        },
        new() {
            Name = "Inclusive Tech Inc.",
            Description = "Focused on accessibility in technology.",
            Logo = "http://localhost:8089/Logos/InclusiveTech.png"
        }
    });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedRolesAndUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {
        if (!await roleManager.Roles.AnyAsync())
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Freelancer"));
            await roleManager.CreateAsync(new IdentityRole("Employer"));
        }

        if (await userManager.FindByEmailAsync("employer@hireup.com") == null)
        {
            var employerUser = new ApplicationUser { UserName = "employer@hireup.com", Email = "employer@hireup.com", FirstName = "John", EmailConfirmed = true };
            await userManager.CreateAsync(employerUser, "Password123!");
            await userManager.AddToRoleAsync(employerUser, "Employer");
        }
    }

    private static async Task SeedFeaturesAsync(ApplicationDbContext context)
    {
        if (await context.JobListings.AnyAsync()) return;

        var employer = await context.Users.FirstAsync(u => u.Email == "employer@hireup.com");
        var fbCompany = await context.Companies.FirstAsync(c => c.Name == "Facebook");
        var googleCompany = await context.Companies.FirstAsync(c => c.Name == "Google");
        var incTechCompany = await context.Companies.FirstAsync(c => c.Name == "Inclusive Tech Inc.");

        var seniorLevel = await context.ExperienceLevels.FirstAsync(l => l.Name == "Senior");
        var entryLevel = await context.ExperienceLevels.FirstAsync(l => l.Name == "Entry-Level");

        var itCategory = await context.JobCategories.FirstAsync(cat => cat.Name == "IT");
        var marketingCategory = await context.JobCategories.FirstAsync(cat => cat.Name == "Marketing");

        var fullTimeType = await context.JobTypes.FirstAsync(t => t.Name == "Full-time");
        var internType = await context.JobTypes.FirstAsync(t => t.Name == "Internship");

        var jobListings = new List<JobListing>
        {
            new() {
                Title = "Senior Software Engineer",
                Description = "Join Facebook team...",
                Requirements = "5+ years C# experience",
                CompanyId = fbCompany.Id,
                ExperienceLevelId = seniorLevel.Id,
                JobCategoryId = itCategory.Id,
                JobTypeId = fullTimeType.Id, 
                EmployerId = employer.Id,
                Location = "California, USA",
                Salary = 180000,
                IsFeatured = true,
                ExpiryDate = DateTime.UtcNow.AddDays(30)
            },
            new() {
                Title = "Marketing Intern",
                Description = "Learn marketing at Inclusive Tech...",
                Requirements = "Proactive attitude",
                CompanyId = incTechCompany.Id,
                ExperienceLevelId = entryLevel.Id,
                JobCategoryId = marketingCategory.Id,
                JobTypeId = internType.Id, 
                EmployerId = employer.Id,
                Location = "Remote",
                Salary = 45000,
                IsFeatured = false,
                ExpiryDate = DateTime.UtcNow.AddDays(15)
            }
        };

        await context.JobListings.AddRangeAsync(jobListings);
        await context.SaveChangesAsync();
    }
}