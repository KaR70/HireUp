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

        public JobType Type { get; set; }
        public decimal? Salary { get; set; }

        public string? CompanyName { get; set; } 

        public bool IsInclusiveHiring { get; set; }
        public string? DisabilitySupport { get; set; }  

        public string EmployerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ApplicationUser Employer { get; set; } = null!;  
        public ICollection<Skill> RequiredSkills { get; set; } = new List<Skill>();
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }

    public enum JobType
    {
        FullTime, PartTime, Contract, Freelance, Remote
    }
}