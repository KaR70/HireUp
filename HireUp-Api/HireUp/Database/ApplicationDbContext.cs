using HireUp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace HireUp.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Skill> Skills { get; set; }
    public DbSet<JobListing> JobListings { get; set; }
    public DbSet<MockInterview> MockInterviews { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<DisabilityType> DisabilityTypes { get; set; }
    public DbSet<UserDisabilityType> UserDisabilityTypes { get; set; }
    public DbSet<AccessibilityNeed> AccessibilityNeed { get; set; }
    public DbSet<UserAccessibilityNeed> UserAccessibilityNeed { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<ExperienceLevel> ExperienceLevels { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }

    // الجداول الجديدة (شغل النهاردة - ضفناها هنا)
    public DbSet<JobType> JobTypes { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<OfficeType> OfficeTypes { get; set; }
    public DbSet<JobRole> JobRoles { get; set; }
    public DbSet<UserJobCategoryPreference> UserJobCategoryPreferences { get; set; }
    public DbSet<UserJobTypePreference> UserJobTypePreferences { get; set; }
    public DbSet<UserLocationPreference> UserLocationPreferences { get; set; }
    public DbSet<UserOfficeTypePreference> UserOfficeTypePreferences { get; set; }
    public DbSet<Review> Review { get; set; }
    public DbSet<Follows> Follows { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<SavedJob> SavedJobs { get; set; }
    public DbSet<Industry> Industries { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // يفضل وضعها في البداية عند استخدام Identity

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<UserJobCategoryPreference>().HasKey(pc => new { pc.UserId, pc.JobCategoryId });
            builder.Entity<UserJobTypePreference>().HasKey(pt => new { pt.UserId, pt.JobTypeId });
            builder.Entity<UserLocationPreference>().HasKey(pl => new { pl.UserId, pl.LocationId });
            builder.Entity<UserOfficeTypePreference>().HasKey(po => new { po.UserId, po.OfficeTypeId });

            // Many-to-Many: Users <-> Skills
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Skills)
                .WithMany(s => s.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserSkills",
                    j => j.HasOne<Skill>().WithMany().HasForeignKey("SkillId"),
                    j => j.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId"),
                    j => j.HasKey("UserId", "SkillId"));

            // Many-to-Many: JobListings <-> Skills
            builder.Entity<JobListing>()
                .HasMany(j => j.RequiredSkills)
                .WithMany(s => s.JobListings)
                .UsingEntity<Dictionary<string, object>>(
                    "JobListingSkills",
                    j => j.HasOne<Skill>().WithMany().HasForeignKey("SkillId"),
                    j => j.HasOne<JobListing>().WithMany().HasForeignKey("JobListingId"),
                    j => j.HasKey("JobListingId", "SkillId"));

            // Configure JobListing
            builder.Entity<JobListing>(entity =>
            {
                entity.HasKey(j => j.Id);
                entity.Property(j => j.Title).IsRequired().HasMaxLength(200);
                entity.Property(j => j.Description).IsRequired();
                entity.Property(j => j.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(j => j.Employer)
                      .WithMany(u => u.JobListings)
                      .HasForeignKey(j => j.EmployerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure MockInterview
            builder.Entity<MockInterview>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Title).IsRequired().HasMaxLength(200);
                entity.Property(m => m.Industry).IsRequired().HasMaxLength(100);

                entity.HasOne(m => m.JobSeeker)
                      .WithMany(u => u.InterviewsAsSeeker)
                      .HasForeignKey(m => m.JobSeekerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Interviewer)
                      .WithMany(u => u.InterviewsAsInterviewer)
                      .HasForeignKey(m => m.InterviewerId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure JobApplication
            builder.Entity<JobApplication>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.CoverLetter).IsRequired();
                entity.Property(a => a.AppliedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(a => a.JobListing)
                      .WithMany(j => j.Applications)
                      .HasForeignKey(a => a.JobListingId)
                      .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.JobSeeker)
                  .WithMany(u => u.Applications)
                  .HasForeignKey(a => a.JobSeekerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}