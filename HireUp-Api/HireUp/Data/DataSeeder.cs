using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HireUp.Database;     // IMPORTANT: Change to your DbContext's namespace
using HireUp.Entities; // IMPORTANT: Change to your Entities' namespace

public static class DataSeeder
{
    // Main method to orchestrate all seeding
    public static async Task SeedAllAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); // Change to your DbContext name
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        await SeedLookupsAsync(context);
        await SeedRolesAndUsersAsync(userManager, roleManager, context);
        await SeedFeaturesAsync(context);
    }

    // LEVEL 0: Foundation Entities
    private static async Task SeedLookupsAsync(ApplicationDbContext context)
    {
        if (!await context.Skills.AnyAsync())
        {
            var skills = new List<Skill>
            {
                new() { Name = "C#", Description = "Object-oriented programming language by Microsoft.", Category = SkillCategory.Technical },
                new() { Name = "ASP.NET Core", Description = "A cross-platform, high-performance, open-source framework for building modern, cloud-enabled, Internet-connected apps.", Category = SkillCategory.Technical },
                new() { Name = "React", Description = "A JavaScript library for building user interfaces.", Category = SkillCategory.Technical },
                new() { Name = "UI/UX Design", Description = "Designing user interfaces and user experiences for digital products.", Category = SkillCategory.Professional },
                new() { Name = "Copywriting", Description = "Writing text for the purpose of advertising or other forms of marketing.", Category = SkillCategory.Professional },
                new() { Name = "Woodworking", Description = "The skill of making items from wood.", Category = SkillCategory.Craft }
            };
            await context.Skills.AddRangeAsync(skills);
        }

        if (!await context.DisabilityTypes.AnyAsync())
        {
            var disabilityTypes = new List<DisabilityType>
            {
                new() { Name = "Mobility", Description = "Affects a person's ability to move or use their limbs." },
                new() { Name = "Vision", Description = "Affects a person's sight." },
                new() { Name = "Hearing", Description = "Affects a person's ability to hear." },
                new() { Name = "Cognitive", Description = "Affects a person's ability to learn, remember, or make decisions." }
            };
            await context.DisabilityTypes.AddRangeAsync(disabilityTypes);
        }

        if (!await context.AccessibilityNeed.AnyAsync())
        {
            var accessibilityNeeds = new List<AccessibilityNeed>
            {
                new() { Name = "Wheelchair Accessible Workplace", Description = "Premises are accessible to wheelchair users." },
                new() { Name = "Screen Reader Software", Description = "Requires compatibility with software that reads screen content aloud." },
                new() { Name = "Flexible Work Hours", Description = "Allows for adjustments to the standard 9-to-5 workday." },
                new() { Name = "Sign Language Interpretation", Description = "Requires availability of a sign language interpreter for meetings." }
            };
            await context.AccessibilityNeed.AddRangeAsync(accessibilityNeeds);
        }

        await context.SaveChangesAsync();
    }

    // LEVEL 1: Core Entity (Users & Roles)
    private static async Task SeedRolesAndUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {
        if (!await roleManager.Roles.AnyAsync())
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Freelancer"));
            await roleManager.CreateAsync(new IdentityRole("Employer"));
        }

        if (await userManager.FindByEmailAsync("admin@hireup.com") == null)
        {
            var adminUser = new ApplicationUser { UserName = "admin@hireup.com", Email = "admin@hireup.com", FirstName = "Admin", LastName = "User", EmailConfirmed = true };
            await userManager.CreateAsync(adminUser, "Password123!");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        
        if (await userManager.FindByEmailAsync("freelancer@hireup.com") == null)
        {
            var freelancerUser = new ApplicationUser { UserName = "freelancer@hireup.com", Email = "freelancer@hireup.com", FirstName = "Jane", LastName = "Doe", EmailConfirmed = true, Bio = "Experienced .NET developer with a passion for accessible design." };
            await userManager.CreateAsync(freelancerUser, "Password123!");
            await userManager.AddToRoleAsync(freelancerUser, "Freelancer");

            var csharpSkill = await context.Skills.FirstAsync(s => s.Name == "C#");
            var reactSkill = await context.Skills.FirstAsync(s => s.Name == "React");
            var visionDisability = await context.DisabilityTypes.FirstAsync(d => d.Name == "Vision");
            var screenReaderNeed = await context.AccessibilityNeed.FirstAsync(a => a.Name == "Screen Reader Software");

            freelancerUser.Skills.Add(csharpSkill);
            freelancerUser.Skills.Add(reactSkill);
            freelancerUser.UserDisabilityTypes.Add(new UserDisabilityType { DisabilityTypeId = visionDisability.Id });
            freelancerUser.UserAccessibilityNeeds.Add(new UserAccessibilityNeed { AccessibilityNeedId = screenReaderNeed.Id });

            await userManager.UpdateAsync(freelancerUser);
        }

        if (await userManager.FindByEmailAsync("employer@hireup.com") == null)
        {
            var employerUser = new ApplicationUser { UserName = "employer@hireup.com", Email = "employer@hireup.com", FirstName = "John", LastName = "Smith", EmailConfirmed = true, Bio = "Hiring manager at a tech startup focused on inclusive products." };
            await userManager.CreateAsync(employerUser, "Password123!");
            await userManager.AddToRoleAsync(employerUser, "Employer");
        }

        if (await userManager.FindByEmailAsync("expert@hireup.com") == null)
        {
            var expertUser = new ApplicationUser { UserName = "expert@hireup.com", Email = "expert@hireup.com", FirstName = "Alex", LastName = "Chen", EmailConfirmed = true, Bio = "Senior Engineer and certified interviewer, specializing in technical assessments." };
            await userManager.CreateAsync(expertUser, "Password123!");
            await userManager.AddToRoleAsync(expertUser, "Freelancer"); // Can be a freelancer who also does interviews
        }
    }

    // LEVEL 2 & 3: Dependent Entities
    private static async Task SeedFeaturesAsync(ApplicationDbContext context)
    {
        if (!await context.JobListings.AnyAsync())
        {
            var employer = await context.Users.FirstAsync(u => u.Email == "employer@hireup.com");
            var csharpSkill = await context.Skills.FirstAsync(s => s.Name == "C#");
            var aspnetSkill = await context.Skills.FirstAsync(s => s.Name == "ASP.NET Core");

            var jobListing = new JobListing
            {
                Title = "Senior Full-Stack Developer",
                Description = "Looking for a talented developer to join our team. Experience with accessible front-end frameworks is a plus.",
                Location = "Remote",
                Type = JobType.FullTime,
                Salary = 120000,
                CompanyName = "Inclusive Tech Inc.",
                IsInclusiveHiring = true,
                DisabilitySupport = "We provide all necessary hardware and software, including screen readers and ergonomic equipment.",
                EmployerId = employer.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                RequiredSkills = new List<Skill> { csharpSkill, aspnetSkill }
            };
            await context.JobListings.AddAsync(jobListing);
            await context.SaveChangesAsync(); // Save to get the JobListing ID

            var freelancer = await context.Users.FirstAsync(u => u.Email == "freelancer@hireup.com");
            var application = new JobApplication
            {
                JobListingId = jobListing.Id,
                JobSeekerId = freelancer.Id,
                CoverLetter = "I am very excited about this opportunity. My background in ASP.NET Core and my personal experience with assistive technologies make me a strong candidate.",
                Status = ApplicationStatus.Pending
            };
            await context.Applications.AddAsync(application);

            var expert = await context.Users.FirstAsync(u => u.Email == "expert@hireup.com");
            var mockInterview = new MockInterview
            {
                Title = "Technical Interview Practice for Senior .NET Role",
                Industry = "Software Development",
                ScheduledAt = DateTime.UtcNow.AddDays(7),
                DurationMinutes = 60,
                JobSeekerId = freelancer.Id,
                InterviewerId = expert.Id,
                Status = InterviewStatus.Scheduled
            };
            await context.MockInterviews.AddAsync(mockInterview);

            await context.SaveChangesAsync();
        }
    }
}