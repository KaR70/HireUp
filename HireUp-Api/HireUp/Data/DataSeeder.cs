using HireUp.Database;

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
        
        if (!await context.Companies.AnyAsync())
        {
            var companies = new List<Company>
            {
                new() {
                    Name = "Facebook",
                    Description = "A social networking service that allows users to connect with friends and family.",
                    Logo = "http://localhost:8089/Logos/Facebook.png"
                },
                new() {
                    Name = "Google",
                    Description = "A multinational technology company that specializes in Internet-related services and products.",
                    Logo = "http://localhost:8089/Logos/Google.png"
                },
                new() {
                    Name = "Burger King",
                    Description = "A global chain of fast-food restaurants, famous for its flame-grilled burgers.",
                    Logo = "http://localhost:8089/Logos/BurgerKing.png"
                },
                new() {
                    Name = "Beats",
                    Description = "A leading audio brand founded in 2006 by Dr. Dre and Jimmy Iovine.",
                    Logo = "http://localhost:8089/Logos/Beats.png"
                },
                new() {
                    Name = "Inclusive Tech Inc.",
                    Description = "A forward-thinking technology company dedicated to building accessible products for everyone.",
                    Logo = "http://localhost:8089/Logos/InclusiveTech.png"
                }
            };
            await context.Companies.AddRangeAsync(companies);
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
        // We only seed if the JobListings table is completely empty.
        if (await context.JobListings.AnyAsync())
        {
            return;
        }

        // 1. Get all the dependencies we need in one go.
        var employer = await context.Users.FirstAsync(u => u.Email == "employer@hireup.com");
        var companies = await context.Companies.ToListAsync();
        var levels = await context.ExperienceLevels.ToListAsync();
        var categories = await context.JobCategories.ToListAsync();

        var jobListings = new List<JobListing>();

        // --- 2. Create the 3 FEATURED Job Listings ---
        jobListings.Add(new JobListing {
            Title = "Senior Software Engineer", 
            Description = "We are the teams who create all of Facebook's products used by billions of people around the world. Want to build new features and improve existing products like Messenger, Video, Groups, News Feed, Search and more?\n\nResponsibilities: - Full stack web/mobile application development with a variety of coding languages\n- Create consumer products and features using internal programming language Hack\n- Implement web or mobile interfaces using XHTML, CSS, and JavaScript", 
            Requirements = "5+ years of professional experience with C# and .NET.\n- Strong understanding of object-oriented programming.\n- Experience with cloud platforms like Azure or AWS.\n- Familiarity with SQL and database design.\n- Excellent problem-solving skills.",
            CompanyId = companies.First(c => c.Name == "Facebook").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Senior").Id,
            JobCategoryId = categories.First(cat => cat.Name == "IT").Id,
            IsFeatured = true, ViewCount = 150, Salary = 180000, EmployerId = employer.Id,
            Location = "California, USA", Type = JobType.FullTime, ExpiryDate = DateTime.UtcNow.AddDays(30)
        });

        jobListings.Add(new JobListing {
            Title = "Full-Stack Developer", 
            Description = "At Google, you'll work on projects that transform how people connect, play, and do business. Join a dynamic team dedicated to building intuitive and powerful design tools that are used by millions of creators and developers.\n\nResponsibilities:\n- Develop and maintain both client-side (React) and server-side components for our web applications.\n- Integrate with various internal and external APIs to enhance functionality.\n- Participate in the entire product lifecycle, from ideation and prototyping to deployment and maintenance.\n- Ensure the technical feasibility of UI/UX designs.", 
            Requirements = "Proven experience with React and modern JavaScript frameworks.\n- Solid understanding of backend development with Node.js or similar.\n- Experience consuming and designing RESTful APIs.\n- Knowledge of Figma APIs is a huge plus.",
            CompanyId = companies.First(c => c.Name == "Google").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Mid-Level").Id,
            JobCategoryId = categories.First(cat => cat.Name == "Design").Id,
            IsFeatured = true, ViewCount = 125, Salary = 160000, EmployerId = employer.Id,
            Location = "Seattle, USA", Type = JobType.FullTime, ExpiryDate = DateTime.UtcNow.AddDays(30)
        });

        jobListings.Add(new JobListing {
            Title = "Product Manager (Audio)", 
            Description = "Beats is looking for a visionary Product Manager to lead the next generation of our world-class audio products. You will be responsible for defining the product vision, strategy, and roadmap, ensuring we continue to innovate.\n\nResponsibilities:\n- Define the product strategy and roadmap based on market analysis.\n- Gather and prioritize product and customer requirements.\n- Work closely with engineering, design, and marketing teams.\n- Act as the primary evangelist for your product, both internally and externally.", 
            Requirements = "3+ years of experience in product management, preferably in consumer electronics.\n- Strong market analysis and user research skills.\n- Experience with the full product lifecycle, from concept to launch.\n- Ability to work closely with engineering, design, and marketing teams.",
            CompanyId = companies.First(c => c.Name == "Beats").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Senior").Id,
            JobCategoryId = categories.First(cat => cat.Name == "Marketing").Id,
            IsFeatured = true, ViewCount = 95, Salary = 140000, EmployerId = employer.Id,
            Location = "Florida, US", Type = JobType.FullTime, ExpiryDate = DateTime.UtcNow.AddDays(30)
        });

        // --- 3. Create the 7 NON-FEATURED Job Listings ---
        jobListings.Add(new JobListing {
            Title = "Jr Executive", 
            Description = "Kickstart your corporate career with Burger King in this entry-level executive role. You will support our senior management team and gain invaluable experience in a fast-paced corporate environment.\n\nResponsibilities:\n- Assist senior executives with daily administrative duties and special projects.\n- Prepare reports, presentations, and correspondence.\n- Manage calendars, schedule meetings, and coordinate travel arrangements.\n- Handle confidential information with discretion and professionalism.", 
            Requirements = "Bachelor's degree in Business, Communications, or a related field.\n- Strong proficiency in the Microsoft Office Suite (Word, Excel, PowerPoint).\n- Excellent written and verbal communication skills.\n- Ability to multitask and manage time effectively in a fast-paced environment.",
            CompanyId = companies.First(c => c.Name == "Burger King").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Junior").Id,
            JobCategoryId = categories.First(cat => cat.Name == "Marketing").Id,
            IsFeatured = false, ViewCount = 88, Salary = 96000, EmployerId = employer.Id,
            Location = "Los Angeles, US", Type = JobType.FullTime, ExpiryDate = DateTime.UtcNow.AddDays(30)
        });

        jobListings.Add(new JobListing {
            Title = "Product Manager (Search)", 
            Description = "Take on the challenge of improving the world's most used product. As a Product Manager for Google Search, you will be at the forefront of innovation, making data-driven decisions that affect billions of users.\n\nResponsibilities:\n- Drive product development for new features on Google Search from concept to launch.\n- Analyze user data, conduct A/B tests, and perform market research.\n- Define and analyze metrics that inform the success of products.\n- Collaborate with world-class engineers, researchers, and designers.", 
            Requirements = "5+ years in a product management role for a large-scale consumer product.\n- Strong background in data analysis, A/B testing, and statistical validation.\n- Ability to translate complex technical concepts into clear product requirements.\n- Familiarity with machine learning concepts is a plus.",
            CompanyId = companies.First(c => c.Name == "Google").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Senior").Id,
            JobCategoryId = categories.First(cat => cat.Name == "Marketing").Id,
            IsFeatured = false, ViewCount = 84, Salary = 184000, EmployerId = employer.Id,
            Location = "Florida, US", Type = JobType.FullTime, ExpiryDate = DateTime.UtcNow.AddDays(30)
        });

         jobListings.Add(new JobListing {
            Title = "Jr Executive", 
            Description = "This is a second opening for our popular entry-level executive role. You will support our senior management team and gain invaluable experience in a fast-paced corporate environment.\n\nResponsibilities:\n- Assist senior executives with daily administrative duties and special projects.\n- Prepare reports, presentations, and correspondence.\n- Manage calendars, schedule meetings, and coordinate travel arrangements.\n- Handle confidential information with discretion and professionalism.", 
            Requirements = "Bachelor's degree in Business, Communications, or a related field.\n- Strong proficiency in the Microsoft Office Suite (Word, Excel, PowerPoint).\n- Excellent written and verbal communication skills.\n- Ability to multitask and manage time effectively in a fast-paced environment.",
            CompanyId = companies.First(c => c.Name == "Burger King").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Junior").Id,
            JobCategoryId = categories.First(cat => cat.Name == "Marketing").Id,
            IsFeatured = false, ViewCount = 80, Salary = 96000, EmployerId = employer.Id,
            Location = "Los Angeles, US", Type = JobType.FullTime, ExpiryDate = DateTime.UtcNow.AddDays(30)
        });

        jobListings.Add(new JobListing {
            Title = "Cloud Solutions Architect", 
            Description = "As a Cloud Solutions Architect, you will be a key technical advisor to our enterprise customers, helping them leverage the power of Google Cloud Platform.\n\nResponsibilities:\n- Work with enterprise customers to design scalable, secure, and resilient cloud solutions.\n- Provide technical guidance on cloud architecture and best practices.\n- Create and present solution designs, and lead technical workshops.\n- Bridge the gap between complex business problems and technology solutions.", 
            Requirements = "Valid AWS Solutions Architect or Azure Solutions Architect Expert certification required.\n- Deep understanding of cloud networking and security principles.\n- Experience with infrastructure as code (Terraform, CloudFormation).\n- Proven track record of designing and deploying resilient, scalable systems.",
            CompanyId = companies.First(c => c.Name == "Google").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Lead").Id,
            JobCategoryId = categories.First(cat => cat.Name == "IT").Id,
            IsFeatured = false, ViewCount = 75, Salary = 220000, EmployerId = employer.Id,
            Location = "Remote", Type = JobType.Remote, ExpiryDate = DateTime.UtcNow.AddDays(45)
        });

        jobListings.Add(new JobListing {
            Title = "Lead DevOps Engineer", 
            Description = "Join the infrastructure team responsible for the reliability and scalability of Facebook's global services. You will automate, build, and manage the critical infrastructure that underpins our products.\n\nResponsibilities:\n- Lead the design and implementation of CI/CD pipelines for various services.\n- Manage and scale our extensive Kubernetes infrastructure.\n- Develop automation scripts for infrastructure provisioning and monitoring.\n- Champion DevOps best practices and a culture of automation.", 
            Requirements = "7+ years in a DevOps or SRE role.\n- Expertise in container orchestration with Kubernetes.\n- Proficiency with infrastructure as code tools, particularly Terraform.\n- Strong scripting skills (Python, Bash).\n- Experience managing large-scale, high-availability production environments.",
            CompanyId = companies.First(c => c.Name == "Facebook").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Lead").Id,
            JobCategoryId = categories.First(cat => cat.Name == "IT").Id,
            IsFeatured = false, ViewCount = 62, Salary = 210000, EmployerId = employer.Id,
            Location = "Remote", Type = JobType.Remote, ExpiryDate = DateTime.UtcNow.AddDays(20)
        });

        jobListings.Add(new JobListing {
            Title = "UX/UI Designer", 
            Description = "At Beats, design is at the core of our products. We are looking for a talented UX/UI Designer to create intuitive, accessible, and beautiful user experiences for our mobile and web platforms.\n\nResponsibilities:\n- Translate concepts into user flows, wireframes, mockups, and prototypes.\n- Conduct user research and evaluate user feedback to enhance designs.\n- Establish and promote design guidelines, best practices, and standards.\n- Collaborate with product managers and engineers to implement innovative solutions.", 
            Requirements = "A strong portfolio showcasing your design process and final products is required.\n- Proficiency in Figma, Sketch, or Adobe XD.\n- Experience creating wireframes, prototypes, and high-fidelity mockups.\n- A deep understanding of accessibility standards (WCAG).",
            CompanyId = companies.First(c => c.Name == "Beats").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Mid-Level").Id,
            JobCategoryId = categories.First(cat => cat.Name == "Design").Id,
            IsFeatured = false, ViewCount = 55, Salary = 110000, EmployerId = employer.Id,
            Location = "California, USA", Type = JobType.FullTime, ExpiryDate = DateTime.UtcNow.AddDays(60)
        });

        jobListings.Add(new JobListing {
            Title = "Marketing Intern",
            Description = "This is a unique opportunity for an aspiring marketer to gain hands-on experience at a forward-thinking tech company. As a Marketing Intern, you will support our team's efforts in content creation, social media, and digital campaigns.\n\nResponsibilities:\n- Assist in the creation of email campaigns, social media content, and blog posts.\n- Conduct market research on competitors and industry trends.\n- Help organize and promote marketing events and webinars.\n- Support the marketing team in daily administrative tasks.", 
            Requirements = "Must be currently enrolled in a Bachelor's or Master's degree program in Marketing, Communications, or a related field.\n- Strong desire to learn and a proactive attitude.\n- Familiarity with social media platforms for business.\n- Excellent communication skills.",
            CompanyId = companies.First(c => c.Name == "Inclusive Tech Inc.").Id,
            ExperienceLevelId = levels.First(l => l.Name == "Entry-Level").Id,
            JobCategoryId = categories.First(cat => cat.Name == "Marketing").Id,
            IsFeatured = false, ViewCount = 40, Salary = 45000, EmployerId = employer.Id,
            Location = "Remote", Type = JobType.Contract, ExpiryDate = DateTime.UtcNow.AddDays(15)
        });

        // 4. Add the entire list to the context in one go and save.
        await context.JobListings.AddRangeAsync(jobListings);
        await context.SaveChangesAsync();
    }
}