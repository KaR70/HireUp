using HireUp.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HireUp.Entities
{
    public class JobListing
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        public decimal? Salary { get; set; }

        public bool IsInclusiveHiring { get; set; }
        public string? DisabilitySupport { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; }
        public int ViewCount { get; set; }
        public string Requirements { get; set; }

        public string EmployerId { get; set; }
        public int ExperienceLevelId { get; set; }
        public int CompanyId { get; set; }
        public int JobCategoryId { get; set; }

       
        public int JobTypeId { get; set; } 
        public JobType JobType { get; set; } 
        
        public ApplicationUser Employer { get; set; } = null!;
        public ExperienceLevel ExperienceLevel { get; set; }
        public Company Company { get; set; }
        public JobCategory JobCategory { get; set; }
        public ICollection<Skill> RequiredSkills { get; set; } = new List<Skill>();
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }

   
}